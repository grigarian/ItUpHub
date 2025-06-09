#!/bin/bash

# Production deployment script for ItUpHub

set -e

echo "🚀 Starting ItUpHub production deployment..."

# Check if .env file exists
if [ ! -f .env ]; then
    echo "❌ Error: .env file not found!"
    echo "Please copy env.prod.example to .env and configure your environment variables."
    exit 1
fi

# Load environment variables
source .env

# Validate required environment variables
required_vars=("DB_USER" "DB_PASSWORD" "JWT_SECRET_KEY" "MINIO_ACCESS_KEY" "MINIO_SECRET_KEY" "FRONTEND_URL" "BACKEND_URL")
for var in "${required_vars[@]}"; do
    if [ -z "${!var}" ]; then
        echo "❌ Error: $var is not set in .env file"
        exit 1
    fi
done

echo "✅ Environment variables validated"

# Stop existing containers
echo "🛑 Stopping existing containers..."
docker-compose -f docker-compose.prod.yml down

# Remove old images
echo "🧹 Cleaning up old images..."
docker system prune -f

# Build and start services
echo "🔨 Building and starting services..."
docker-compose -f docker-compose.prod.yml up --build -d

# Wait for services to be healthy
echo "⏳ Waiting for services to be healthy..."
sleep 30

# Check service health
echo "🏥 Checking service health..."
if docker-compose -f docker-compose.prod.yml ps | grep -q "unhealthy"; then
    echo "❌ Some services are unhealthy. Check logs:"
    docker-compose -f docker-compose.prod.yml logs
    exit 1
fi

echo "✅ All services are healthy!"

# Run database migrations (if needed)
echo "🗄️ Running database migrations..."
docker-compose -f docker-compose.prod.yml exec backend dotnet ef database update

echo "🎉 Deployment completed successfully!"
echo "🌐 Frontend: $FRONTEND_URL"
echo "🔧 Backend API: $BACKEND_URL"
echo "📊 MinIO Console: $FRONTEND_URL:9001" 