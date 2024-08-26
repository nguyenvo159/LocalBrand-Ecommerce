import sys
import torch
import torchvision.transforms as transforms
from torchvision.models import resnet50, ResNet50_Weights
from PIL import Image
import json

def load_image(image_path, transform=None):
    image = Image.open(image_path).convert("RGB")
    if transform:
        image = transform(image).unsqueeze(0)
    return image

def extract_features(image_path):
    model = resnet50(weights=ResNet50_Weights.IMAGENET1K_V1)
    model = torch.nn.Sequential(*list(model.children())[:-1])
    model.eval()

    preprocess = transforms.Compose([
        transforms.Resize(256),
        transforms.CenterCrop(224),
        transforms.ToTensor(),
        transforms.Normalize(mean=[0.485, 0.456, 0.406], std=[0.229, 0.224, 0.225]),
    ])

    image = load_image(image_path, preprocess)
    with torch.no_grad():
        features = model(image).squeeze().numpy()

    return json.dumps(features.tolist())

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python script.py <image_path>")
        sys.exit(1)
    
    image_path = sys.argv[1]
    image_vector = extract_features(image_path)
    print(image_vector)
