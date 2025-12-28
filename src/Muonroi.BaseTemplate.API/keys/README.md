# RSA Keys for JWT Authentication

This folder contains RSA key files for JWT token signing/verification.

## Generate New Keys

```bash
# Generate private key (2048 bits recommended for production)
openssl genrsa -out private.pem 2048

# Extract public key from private key
openssl rsa -in private.pem -pubout -out public.pem
```

## Configuration

In `appsettings.json`:

```json
{
  "TokenConfigs": {
    "UseRsa": true,
    "PublicKeyPath": "keys/public.pem",
    "PrivateKeyPath": "keys/private.pem"
  }
}
```

## Security Notes

- **NEVER commit real private keys to source control**
- Add `*.pem` to `.gitignore` in production
- Use environment variables or secrets manager for production keys
- The example keys in this folder are for development only

## Alternative: Inline Keys

You can also use inline PEM strings in config (not recommended for production):

```json
{
  "TokenConfigs": {
    "UseRsa": true,
    "PublicKey": "-----BEGIN PUBLIC KEY-----\nMIGf...==\n-----END PUBLIC KEY-----",
    "PrivateKey": "-----BEGIN RSA PRIVATE KEY-----\nMIIC...==\n-----END RSA PRIVATE KEY-----"
  }
}
```
