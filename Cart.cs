# E-Commerce Multi-Vendor Platform

Sistem i avancuar E-Commerce me arkitekturë të shtresuar (Layered Architecture), Repository Pattern dhe parimet SOLID.

## Teknologjia
- **Gjuha:** C# (.NET 8)
- **Ruajtja:** File CSV (Repository Pattern)
- **Arkitektura:** Layered Architecture + SOLID

## Struktura e Projektit

```
ECommerceProject/
├── Models/          → Strukturat e të dhënave
├── Services/        → Logjika e biznesit
│   └── Interfaces/  → Abstraksionet (ISP)
├── Data/            → Repository Pattern
├── UI/              → Ndërfaqja e përdoruesit
├── docs/            → Dokumentimi
└── Program.cs       → Inicializim minimal
```

## Funksionalitetet Kryesore

- **Multi-Vendor System** — shumë shitës me dyqane të izoluara
- **ID Unike + QR Code** — çdo dyqan ka identitet unik
- **Shopping Cart** — shportë dinamike me total automatik
- **Order Management** — porosi me status dhe njoftime
- **Review & Rating** — vlerësime me yje
- **Gift Cards** — karta dhurate me kod unik
- **Analytics** — statistika shitjesh për admin
- **Advanced Search & Filter** — kërkim me filtra të avancuara
- **Autentikim i Sigurt** — SHA-256 hashing

## Parimet SOLID

| Parimi | Zbatimi |
|--------|---------|
| SRP | Çdo shtresë ka një përgjegjësi të vetme |
| OCP | `IRepository<T>` lejon zgjerim pa modifikim |
| LSP | Çdo implementim i `IRepository<T>` është i ndërrueshem |
| ISP | Interface të ndara për çdo shërbim |
| DIP | Services varen nga interfaces, jo nga implementime |

## Ekzekutimi

```bash
cd ECommerceProject
dotnet run
```

## Dokumentimi

- [Arkitektura e Projektit](docs/architecture.md)
- [Diagrami i Klasave UML](docs/class-diagram.md)
