﻿using System.Text.Json;
using System.Text.RegularExpressions;
using AutoMapper;
using backend.Dto.Common;
using backend.Dto.Review;
using backend.Entity;
using backend.Repositories;

namespace backend.Services;

public class ReviewService : IReviewService
{
    private readonly IRepository<Review> _reviewRepository;
    private readonly IMapper _mapper;

    public ReviewService(IRepository<Review> reviewRepository, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    public async Task<ReviewDto?> GetById(Guid id)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        if (review == null)
        {
            throw new ApplicationException("Review not found");
        }
        return _mapper.Map<ReviewDto>(review);
    }

    public async Task<List<ReviewDto>> GetByProductId(Guid productId)
    {
        var reviews = await _reviewRepository.FindAllAsync(r => r.ProductId == productId);
        return _mapper.Map<List<ReviewDto>>(reviews ?? new List<Review>());
    }
    public async Task<ReviewDto> Create(ReviewCreateDto reviewCreateDto)
    {
        var existReview = await _reviewRepository.FindAsync(x => x.UserId == reviewCreateDto.UserId && x.ProductId == reviewCreateDto.ProductId);
        if (existReview != null)
        {
            throw new ApplicationException("User is voted product");
        }
        var review = _mapper.Map<Review>(reviewCreateDto);
        review.Comment = FilterBadWords(review.Comment);
        await _reviewRepository.AddAsync(review);
        return _mapper.Map<ReviewDto>(review);
    }
    public async Task<ReviewDto> Update(ReviewUpdateDto reviewUpdateDto)
    {
        var existReview = await _reviewRepository.GetByIdAsync(reviewUpdateDto.Id);
        if (existReview == null)
        {
            throw new ApplicationException("Review not found");
        }
        _mapper.Map(reviewUpdateDto, existReview);
        existReview.Comment = FilterBadWords(existReview.Comment);
        await _reviewRepository.UpdateAsync(existReview);
        return _mapper.Map<ReviewDto>(existReview);
    }

    public async Task Delete(Guid id)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        if (review == null)
        {
            throw new ApplicationException("Review not found");
        }
        await _reviewRepository.DeleteAsync(id);
    }

    public Task<ReviewDto?> GetByUserId(Guid userId)
    {
        var review = _reviewRepository.FindAsync(x => x.UserId == userId);
        if (review == null)
        {
            throw new ApplicationException("Review not found");
        }
        return Task.FromResult(_mapper.Map<ReviewDto>(review));
    }

    public Task<PageResult<ReviewDto>> GetPaging(ReviewGetPagingReq request)
    {
        var query = _reviewRepository.AsQueryable();
        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(x => x.Comment.Contains(request.Search) || x.User.Name.Contains(request.Search));
        }
        if (request.Filter.HasValue)
        {
            if (request.Filter.Value == 0)
            {
                query = query.Where(x => x.Rating >= 4);
            }
            else if (request.Filter.Value == 1)
            {
                query = query.Where(x => x.Rating < 4);
            }
        }
        var totalRecords = query.Count();
        query = query.OrderByDescending(x => x.CreatedAt);
        if (request.PageSize.HasValue && request.PageNumber.HasValue)
        {
            query = query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value);
        }
        var data = query.ToList();
        data = data.OrderByDescending(x => x.CreatedAt).ToList();
        var result = new PageResult<ReviewDto>
        {
            TotalRecords = totalRecords,
            Items = _mapper.Map<List<ReviewDto>>(data),
            PageSize = request.PageSize ?? 1,
            PageNumber = request.PageNumber ?? 1
        };
        result.Items = result.Items.OrderByDescending(x => x.CreatedAt).ToList();
        return Task.FromResult(result);
    }
    private string FilterBadWords(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input ?? string.Empty;
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "backend", "Text", "BadWord.json");

        if (!File.Exists(filePath))
        {
            return input;
        }

        var jsonContent = File.ReadAllText(filePath);
        var badWordsData = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonContent);
        var badWords = badWordsData?["badwords"] ?? new List<string>();

        foreach (var word in badWords)
        {
            var pattern = @"\b" + Regex.Escape(word) + @"\b";
            input = Regex.Replace(input, pattern, new string('*', word.Length), RegexOptions.IgnoreCase);
        }

        return input;
    }
}
