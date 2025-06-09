# ItUpHub Production Deployment Guide

## Prerequisites

- Docker and Docker Compose installed
- Domain name configured
- SSL certificate (recommended)
- Server with at least 4GB RAM and 2 CPU cores

## Quick Start

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd ItUpHub
   ```

2. **Configure environment variables**
   ```bash
   cp env.prod.example .env
   # Edit .env with your production values
   ```

3. **Deploy**
   ```bash
   ./deploy.sh
   ```

## Environment Variables

### Required Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `DB_USER` | PostgreSQL username | `ituphub_user` |
| `DB_PASSWORD` | PostgreSQL password | `secure_password_123` |
| `JWT_SECRET_KEY` | JWT signing key (min 32 chars) | `your_very_long_secret_key_here` |
| `MINIO_ACCESS_KEY` | MinIO access key | `ituphub_minio_user` |
| `MINIO_SECRET_KEY` | MinIO secret key | `secure_minio_key_123` |
| `FRONTEND_URL` | Frontend URL | `https://your-domain.com` |
| `BACKEND_URL` | Backend API URL | `https://your-domain.com/api` |

### Security Recommendations

- Use strong, unique passwords
- Generate a secure JWT secret key (at least 32 characters)
- Use HTTPS in production
- Regularly rotate credentials

## SSL Configuration

### Using Let's Encrypt (Recommended)

1. Install Certbot
2. Generate certificate:
   ```bash
   sudo certbot certonly --standalone -d your-domain.com -d www.your-domain.com
   ```

3. Update nginx configuration with certificate paths:
   ```nginx
   ssl_certificate /etc/letsencrypt/live/your-domain.com/fullchain.pem;
   ssl_certificate_key /etc/letsencrypt/live/your-domain.com/privkey.pem;
   ```

### Manual SSL Setup

1. Obtain SSL certificate from your provider
2. Place certificate files in `/etc/ssl/certs/` and `/etc/ssl/private/`
3. Update `nginx/production.conf` with correct paths

## Database Setup

### PostgreSQL

The application uses PostgreSQL. For production:

1. **Use managed database** (recommended):
   - AWS RDS
   - Google Cloud SQL
   - Azure Database for PostgreSQL

2. **Self-hosted**:
   - Configure backups
   - Set up monitoring
   - Use connection pooling

### Backup Strategy

```bash
# Create backup script
#!/bin/bash
docker-compose -f docker-compose.prod.yml exec postgres pg_dump -U $DB_USER grow_sphere > backup_$(date +%Y%m%d_%H%M%S).sql
```

## Monitoring and Logging

### Application Logs

```bash
# View logs
docker-compose -f docker-compose.prod.yml logs -f backend
docker-compose -f docker-compose.prod.yml logs -f frontend

# Log rotation is configured in appsettings.Production.json
```

### Health Checks

- Frontend: `https://your-domain.com/health`
- Backend: `https://your-domain.com/api/health`

### Performance Monitoring

Consider setting up:
- Prometheus + Grafana
- Application Performance Monitoring (APM)
- Error tracking (Sentry, etc.)

## Security Checklist

- [ ] SSL certificate installed
- [ ] Strong passwords configured
- [ ] Firewall rules set
- [ ] Regular security updates
- [ ] Database backups configured
- [ ] Monitoring and alerting set up
- [ ] Rate limiting enabled
- [ ] Security headers configured

## Troubleshooting

### Common Issues

1. **Services not starting**
   ```bash
   docker-compose -f docker-compose.prod.yml logs
   ```

2. **Database connection issues**
   - Check environment variables
   - Verify PostgreSQL is running
   - Check network connectivity

3. **SSL certificate issues**
   - Verify certificate paths
   - Check certificate expiration
   - Test with `openssl s_client`

### Performance Optimization

1. **Enable caching**
   - Configure Redis for session storage
   - Use CDN for static assets

2. **Database optimization**
   - Add indexes
   - Configure connection pooling
   - Regular maintenance

3. **Application optimization**
   - Enable compression
   - Optimize images
   - Minify assets

## Maintenance

### Regular Tasks

1. **Weekly**
   - Check logs for errors
   - Monitor disk space
   - Review security updates

2. **Monthly**
   - Update dependencies
   - Review performance metrics
   - Test backup restoration

3. **Quarterly**
   - Security audit
   - Performance review
   - Update SSL certificates

### Updates

```bash
# Update application
git pull origin main
./deploy.sh

# Update dependencies
docker-compose -f docker-compose.prod.yml build --no-cache
```

## Support

For issues and questions:
- Email: grigarian24@mail.ru
- Telegram: @grigarian24 