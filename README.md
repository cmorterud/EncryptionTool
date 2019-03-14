# EncryptionTool
This is a Windows Presentation Foundation user interface to the AES encryption standard.

# Process
## Encryption
1. Select AES key size (128 or 256).
2. Select key generation method.
	* PBKDF2 generation using a password
	* Generation using random bytes.
	* Base64 key pasted into box.
3. Choose to text mode or file mode.
4. Choose data to encrypt.
	* Either paste text into left-hand box, or choose a file to encrypt using browse file.
5. Click encrypt.

## Decryption
1. Select AES key size to match ciphertext AES key size (128 or 256).
2. Select Base64 key pasted into box.
	* Paste the encryption key into the encryption box.
3. Choose text mode or file mode.
4. Choose data to encrypt.
	* Either paste text into right-hand box, or choose a file to decrypt using browse file.
5. Click decrypt.

# Internals
## Initialization Vector
This implementation pastes the initialization vector
of the AES algorithm into the beginning of the ciphertext,
where the first 16 bytes (AES-128) or first 32 bytes (AES-256)
is the initialization vector used in encrypting.

Note that sending the initialization vector
in the clear is cryptographically secure,
and initialization vectors should never be reused.
Initialization vector generation is built into
this implementation.

## Key
The key upon generation is encoded as a base 64 number for brevity.

## AES Implementation
This interface uses the Microsoft reference AES CBC-PCKS7 implementation.

## PBKDF2 Implementation
This interface uses the RFC-2898 standard Microsoft implementation.
