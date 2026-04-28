# Demo Plan — NexaMarket Commerce Cloud

## 1. Titulli i projektit

**NexaMarket Commerce Cloud**

Platformë multi-vendor e-commerce ku shumë shitës mund të menaxhojnë dyqanet e tyre të veçanta brenda një sistemi të vetëm, me storefront modern dhe seller studio të izoluar për secilin vendor.

## 2. Problemi që zgjidh

Problemi real që zgjidh ky projekt është se shitësit e vegjël ose ekipet që duan të shesin online shpesh kanë dy vështirësi:

- nuk kanë një sistem të vetëm ku mund të menaxhojnë dyqanin shpejt
- ndryshimet në produkte, promo dhe renditje shpesh janë të ngadalta ose konfuze

Ky projekt e zgjidh këtë duke ofruar:

- panel të qartë për shitësin
- storefront të strukturuar për blerësin
- izolim të plotë të të dhënave për secilin shop
- flow të drejtpërdrejtë nga menaxhimi i katalogut te rezultati live në faqe

## 3. Përdoruesit kryesorë

Përdoruesit kryesorë të sistemit janë:

- **shitësi / vendor-i**
  - menaxhon produktet
  - menaxhon fushatat promocionale
  - kontrollon stokun dhe strukturën e katalogut

- **blerësi**
  - kërkon produkte
  - filtron sipas kategorisë dhe stokut
  - shton produkte në shportë
  - sheh totalin e përditësuar automatikisht

- **super admin** në versionin e plotë
  - do të menaxhojë vendorët dhe politikat globale të platformës

## 4. Flow-i që do ta demonstroj live

Flow-i kryesor që do ta demonstroj është:

`seller studio -> CRUD produkti -> storefront -> search/filter -> add to cart -> total quote`

Pse e zgjodha pikërisht këtë flow:

- tregon qartë vlerën e platformës
- lidh anën e shitësit me anën e blerësit
- është i kuptueshëm menjëherë gjatë prezantimit
- demonstron funksionalitet real, jo vetëm UI
- tregon që sistemi është multi-vendor dhe i izoluar

## 5. Një problem real që e kam zgjidhur

**Problemi**

Në projekte e-commerce me shumë pjesë, demo shpesh bëhet konfuz sepse ka shumë ekrane, shumë ide, por pak flow real që lidhet nga fillimi në fund.

**Ku ishte problemi**

Problemi ishte te mungesa e një “vertical slice” të qartë që lidh:

- menaxhimin e katalogut nga shitësi
- shfaqjen e rezultateve te blerësi
- llogaritjen e rezultateve reale në shportë

**Si e zgjidha**

E strukturova projektin rreth një flow-i të vetëm, por të fortë:

- backend në `C# / ASP.NET Core`
- frontend me `TypeScript` source dhe bundle gati për demo
- dy vendorë të seed-uar për të treguar izolimin e store-ve
- kërkim dhe filtra funksionalë
- CRUD për produkte dhe campaign cards
- cart quote i llogaritur nga backend-i

Kjo e kthen projektin nga “shumë ide” në “një demo profesionale që funksionon live”.

## 6. Çka mbetet ende e dobët

Pjesa që ende nuk është aq e fortë sa duhet është:

- nuk ka ende databazë persistente
- nuk ka autentikim real dhe role të plota
- pagesat janë ende në nivel demo, jo gateway real
- nuk ka upload real për imazhe
- marketplace i themes/plugins është vetëm pjesë e roadmap-it

Kjo pjesë do të ishte hapi tjetër pas stabilizimit të flow-it kryesor.

## 7. Struktura e prezantimit (5–7 min)

### Hyrja

- prezantoj emrin e projektit
- shpjegoj çfarë problemi zgjidh
- tregoj për kë është platforma

### Demo live

- hap storefront-in
- tregoj vendor switch
- bëj search dhe filter
- shtoj produkte në cart
- tregoj totalin automatik
- kaloj te seller studio
- editoj një produkt ose campaign
- rikthehem te storefront dhe tregoj ndryshimin live

### Shpjegimi teknik

- backend është ndërtuar me `ASP.NET Core`
- frontend është ndërtuar me `TypeScript`
- sistemi është modeluar si multi-vendor me izolim sipas `vendorId`
- llogaritja e cart-it bëhet nga backend-i

### Problemi + zgjidhja

- problemi: demo e paqartë dhe pa flow real
- zgjidhja: vertical slice i qartë nga admin te storefront

### Mbyllja

- theksoj çfarë funksionon sot
- tregoj çfarë mbetet për përmirësim
- përfundoj me arsyen pse ky projekt mund të rritet në platformë të plotë SaaS

## 8. Demo Readiness

Për ta pasur prezantimin pa konfuzion, do të ndjek këtë rend:

1. hyrja e shkurtër
2. storefront
3. search/filter
4. cart
5. seller studio
6. edit live
7. vendor switch
8. mbyllja

## 9. Plan B

Nëse diçka nuk funksionon live:

- do të përdor README si orientim të shpejtë
- do të tregoj fillimisht seller studio sepse është pjesa më e kontrollueshme
- do të përdor flow-in e përgatitur me produkte seed
- do të shpjegoj logjikën nga kodi dhe dokumentimi në vend të një hapi live
- mund të tregoj edhe vetëm ndryshimin e një produkti dhe reflektimin në storefront, si demo e shkurtër por e fortë

## 10. Çfarë duhet të dorëzoj

- linkun e GitHub repo
- këtë file: `docs/demo-plan.md`

## 11. Mesazhi kryesor i demos

Ky projekt tregon që:

- e kuptoj qartë çfarë po ndërtoj
- di të zgjedh çfarë vlen të prezantohet
- di ta shpjegoj sistemin në mënyrë profesionale
- kam përgatitur projektin për një demo të fokusuar, moderne dhe bindëse
