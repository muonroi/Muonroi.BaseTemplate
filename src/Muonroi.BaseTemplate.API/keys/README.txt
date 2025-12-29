RSA Keys for JWT Token Validation
==================================

Place your RSA key files here:
- public.pem  - Public key for token verification
- private.pem - Private key for token signing

These files will be automatically copied to the output directory (bin/Debug/net9.0/keys)
when you build the project.

To generate RSA keys:
---------------------
Using OpenSSL:
  openssl genrsa -out private.pem 2048
  openssl rsa -in private.pem -pubout -out public.pem

Using .NET:
  var rsa = RSA.Create(2048);
  File.WriteAllText("private.pem", rsa.ExportRSAPrivateKeyPem());
  File.WriteAllText("public.pem", rsa.ExportRSAPublicKeyPem());

Configuration (appsettings.json):
---------------------------------
"TokenConfigs": {
  "UseRsa": true,
  "PublicKeyPath": "keys/public.pem",
  "PrivateKeyPath": "keys/private.pem"
}

Note: Set UseRsa to false to use HMAC-based signing instead.
