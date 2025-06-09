#!/bin/bash

# Generate secure secrets for ItUpHub production deployment

echo "🔐 Generating secure secrets for ItUpHub..."

# Generate JWT secret key (64 characters)
JWT_SECRET=$(openssl rand -hex 32)
echo "JWT_SECRET_KEY=$JWT_SECRET"

# Generate database password (32 characters)
DB_PASSWORD=$(openssl rand -base64 24 | tr -d "=+/" | cut -c1-32)
echo "DB_PASSWORD=$DB_PASSWORD"

# Generate MinIO access key (20 characters)
MINIO_ACCESS_KEY=$(openssl rand -hex 10)
echo "MINIO_ACCESS_KEY=$MINIO_ACCESS_KEY"

# Generate MinIO secret key (32 characters)
MINIO_SECRET_KEY=$(openssl rand -base64 24 | tr -d "=+/" | cut -c1-32)
echo "MINIO_SECRET_KEY=$MINIO_SECRET_KEY"

echo ""
echo "✅ Secrets generated successfully!"
echo ""
echo "📝 Copy these values to your .env file:"
echo ""
echo "DB_USER=ituphub_user"
echo "DB_PASSWORD=$DB_PASSWORD"
echo "JWT_SECRET_KEY=$JWT_SECRET"
echo "MINIO_ACCESS_KEY=$MINIO_ACCESS_KEY"
echo "MINIO_SECRET_KEY=$MINIO_SECRET_KEY"
echo "FRONTEND_URL=https://your-domain.com"
echo "BACKEND_URL=https://your-domain.com/api"
echo ""
echo "⚠️  Keep these secrets secure and never commit them to version control!" 