# ManipalDMS API Foundation

This is the first milestone of the trusted desktop-plus-API architecture.

## Required configuration

Set these values through environment variables or a secret manager:

- `ConnectionStrings__PressDMS`
- `Authentication__Authority`
- `Authentication__Audience`

Never commit production connection strings or tokens.

## Endpoints

- `GET /api/status` — public service status
- `GET /api/status/identity` — authenticated identity and roles
- `GET /health` — SQL Server health status

## Security baseline

The API requires a bearer token issued by a configured OIDC authority. The desktop client must not receive database credentials or bypass API authorization.
