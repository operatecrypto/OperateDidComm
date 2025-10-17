read about https://sunstream.operatedid.com/



Sample DID document:
https://api.accumulateplatform.com/did/sunstream

{
  "@context": [
    "https://www.w3.org/ns/did/v1",
    "https://w3id.org/security/suites/jws-2020/v1"
  ],
  "id": "did:web:sunstream.operatedid.com",
  "controller": "did:web:sunstream.operatedid.com",
  "verificationMethod": [
    {
      "id": "did:web:sunstream.operatedid.com#key-1",
      "type": "JsonWebKey2020",
      "controller": "did:web:sunstream.operatedid.com",
      "publicKeyJwk": {
        "kty": "EC",
        "crv": "secp256k1",
        "x": "MDg1MGM2RkM0QTU5YWM2ODlGOTUxNWMzQ2ZkNEQ4ZDI",
        "y": "NDc1NTdlNGU",
        "use": "sig"
      },
      "blockchainAccountId": "eip155:1:0x0850c6FC4A59ac689F9515c3Cfd4D8d247557e4e"
    }
  ],
  "authentication": [
    "did:web:sunstream.operatedid.com#key-1"
  ],
  "assertionMethod": [
    "did:web:sunstream.operatedid.com#key-1"
  ],
  "service": [
    {
      "id": "did:web:sunstream.operatedid.com#my-services",
      "type": "MyServices",
      "serviceEndpoint": "https://sunstream.operatedid.com/MyServices/index.json",
      "description": "Provides details of all services under the DID"
    },
    {
      "id": "did:web:sunstream.operatedid.com#did-service",
      "type": "DIDService",
      "serviceEndpoint": "https://sunstream.operatedid.com/.well-known/did.json",
      "description": "Link to the DID document"
    },
    {
      "id": "did:web:sunstream.operatedid.com#operate-id",
      "type": "OperateID",
      "serviceEndpoint": "https://sunstream.operatedid.com/operate-id",
      "oid": {
        "id": {
          "publicKeyEth": "0x0850c6FC4A59ac689F9515c3Cfd4D8d247557e4e",
          "publicKey": "2fc5e7192857533a5428ad28d9e9724520242fd9f8286f827b031e06898057bd",
          "publicKeyHash": "9e2f6532f3c0137dd02f54de58633d0605efea3cfe69a3c10441de9018410856",
          "identityUrl": "acc://sunstream.acme"
        },
        "operateKey": "08d834657706803912deaca1c0cb9af88e41937d7425a125a1448916b8c45fa6",
        "signature": "0x32fcea6c7558a903c852ba9da300a0ada5571bb4bf4254bff977e56e7e8b513c5b9a7270b240168ea971c1d013ecfe133894e316adba6d847febd402bb61c5011c"
      },
      "description": "OperateID service with unique OID"
    }
  ],
  "created": "2025-10-15T23:21:49.361Z",
  "updated": "2025-10-15T23:21:49.361Z",
  "proof": {
    "type": "EcdsaSecp256k1Signature2019",
    "created": "2025-10-15T23:21:49.361Z",
    "verificationMethod": "did:web:sunstream.operatedid.com#key-1",
    "proofPurpose": "assertionMethod",
    "jws": "eyJhbGciOiJFUzI1NksiLCJiNjQiOmZhbHNlLCJjcml0IjpbImI2NCJdfQ..MDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwZTIyMzA3ZQ"
  }
}