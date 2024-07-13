using AutoMapper;
using backend.Dto.Cart;
using backend.Entity;
using backend.Repositories;

namespace backend.Services;

public class CartService : ICartService
{
    private readonly IRepository<Cart> _cartRepository;
    private readonly IRepository<CartItem> _cartItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IRepository<Size> _sizeRepository;
    private readonly IRepository<ProductInventory> _inventoryRepository;
    private readonly IMapper _mapper;

    public CartService(IRepository<Cart> cartRepository,
    IRepository<CartItem> cartItemRepository,
    IProductRepository productRepository,
    IRepository<Size> sizeRepository,
    IRepository<ProductInventory> inventoryRepository,
    IMapper mapper)
    {
        _cartRepository = cartRepository;
        _cartItemRepository = cartItemRepository;
        _productRepository = productRepository;
        _sizeRepository = sizeRepository;
        _inventoryRepository = inventoryRepository;
        _mapper = mapper;
    }



    public async Task<CartDto?> GetById(Guid id)
    {
        var cart = await _cartRepository.GetCartAsync(u => u.Id == id);
        if (cart == null)
        {
            throw new ApplicationException("Cart not found");
        }
        await AutoUpdateInventory(cart.CartItems);
        cart = await _cartRepository.GetCartAsync(u => u.Id == id);

        return _mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto?> GetByUserId(Guid userId)
    {
        var cart = await _cartRepository.GetCartAsync(u => u.UserId == userId);
        if (cart == null)
        {
            throw new ApplicationException("Cart not found");
        }

        await AutoUpdateInventory(cart.CartItems);
        cart = await _cartRepository.GetCartAsync(u => u.UserId == userId);
        return _mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto> AddToCart(CartItemCreateDto cartItemCreateDto)
    {
        // Kiểm tra tồn tại Cart
        var cart = await _cartRepository.FindAsync(c => c.UserId == cartItemCreateDto.UserId);
        var inventory = await _inventoryRepository.FindAsync(i => i.ProductId == cartItemCreateDto.ProductId && i.SizeId == cartItemCreateDto.SizeId);
        if (inventory == null || inventory.Inventory == 0)
        {
            throw new ApplicationException("Product is out of stock");
        }

        if (cart == null)
        {
            cart = new Cart { UserId = cartItemCreateDto.UserId };
            await _cartRepository.AddAsync(cart);
        }

        var cartItem = await _cartItemRepository
                            .FindAsync(c => c.ProductId == cartItemCreateDto.ProductId && c.SizeId == cartItemCreateDto.SizeId);
        if (cartItem == null)
        {
            cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = cartItemCreateDto.ProductId,
                SizeId = cartItemCreateDto.SizeId,
                Quantity = cartItemCreateDto.Quantity
            };
            await _cartItemRepository.AddAsync(cartItem);
        }
        else
        {
            if (inventory.Inventory < cartItemCreateDto.Quantity + cartItem.Quantity)
            {
                cartItem.Quantity = inventory.Inventory;
            }
            else
            {
                cartItem.Quantity += cartItemCreateDto.Quantity;
            }
            await _cartItemRepository.UpdateAsync(cartItem);
        }
        return _mapper.Map<CartDto>(cart);
    }

    public async Task<CartItemDto> UpdateCartItem(CartItemUpdateDto cartItemUpdateDto)
    {
        // Kiểm tra tồn tại CartItem
        var cartItem = await _cartItemRepository.GetByIdAsync(cartItemUpdateDto.Id);
        if (cartItem == null)
        {
            throw new ApplicationException("Cart item not found");
        }

        // Kiểm tra số lượng tồn kho
        var inventory = await _inventoryRepository.FindAsync(i => i.ProductId == cartItem.ProductId && i.SizeId == cartItem.SizeId);
        if (inventory == null || inventory.Inventory == 0)
        {
            throw new ApplicationException("Product is out of stock");
        }

        if (cartItemUpdateDto.Quantity > inventory.Inventory)
        {
            cartItemUpdateDto.Quantity = inventory.Inventory;
        }
        else
        {
            cartItem.Quantity = cartItemUpdateDto.Quantity;

        }

        await _cartItemRepository.UpdateAsync(cartItem);
        return _mapper.Map<CartItemDto>(cartItem);
    }

    public async Task RemoveItem(Guid cartItemId)
    {
        var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);
        if (cartItem == null)
        {
            throw new ApplicationException("Cart item not found");
        }
        await _cartItemRepository.DeleteAsync(cartItemId);
    }

    public async Task ClearCart(Guid id)
    {
        var cart = await _cartRepository.GetCartAsync(u => u.Id == id);
        if (cart == null)
        {
            throw new ApplicationException("Cart not found");
        }
        foreach (var item in cart.CartItems)
        {
            var inventory = await _inventoryRepository.FindAsync(i => i.ProductId == item.ProductId && i.SizeId == item.SizeId);
            if (inventory != null && inventory.Inventory >= item.Quantity)
            {
                inventory.Inventory -= item.Quantity;
                await _inventoryRepository.UpdateAsync(inventory);
            }
        }
        await _cartItemRepository.DeleteIfAsync(c => c.CartId == id);
    }

    // Special method

    public async Task AutoUpdateInventory(List<CartItem>? cartItem)
    {
        if (cartItem == null || cartItem.Count == 0)
        {
            throw new ApplicationException("Cart item not found");
        }
        foreach (var item in cartItem)
        {
            var inventory = await _inventoryRepository.FindAsync(i => i.ProductId == item.ProductId && i.SizeId == item.SizeId);
            if (inventory != null)
            {
                if (inventory.Inventory == 0)
                {
                    await _cartItemRepository.DeleteAsync(item.Id);
                    continue;
                }
                item.Quantity = Math.Min(item.Quantity, inventory.Inventory);
                await _cartItemRepository.UpdateAsync(item);
            }
        }
    }
}
