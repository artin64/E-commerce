# Project Audit

## 1. Përshkrimi i shkurtër i projektit

Ky projekt është një platformë E-Commerce që lejon përdoruesit të shikojnë produkte, t’i shtojnë në shportë dhe të simulojnë një proces blerjeje.

Sistemi është ndërtuar me teknologji bazike web (HTML, CSS, c#, typescript) dhe fokusohet në krijimin e një përvoje të thjeshtë dhe funksionale për përdoruesit.

### Përdoruesit kryesorë:
- Blerësit (Users): që eksplorojnë dhe blejnë produkte
- Administratori (në formë bazike): që menaxhon përmbajtjen (produkte, kategori)

### Funksionaliteti kryesor:
- Shfaqja e produkteve
- Shtimi në shportë (cart)
- Menaxhimi i produkteve në UI
- Navigimi nëpër kategori

---

## 2. Çka funksionon mirë?

1. **UI bazike funksionale**
   - Faqja është e navigueshme dhe përdoruesi mund të ndërveprojë me produktet pa probleme kritike.

2. **Logjika e shportës (Cart)**
   - Shtimi dhe menaxhimi i produkteve funksionon në mënyrë të kuptueshme dhe të drejtpërdrejtë.

3. **Strukturë e ndarë në HTML / CSS / JS**
   - Kodi është i ndarë sipas përgjegjësive bazike, që është një praktikë e mirë fillestare.

---

## 3. Dobësitë e projektit

1. **Mungesë e strukturës së qartë arkitekturore**
   - Nuk ka ndarje të qartë mes:
     - UI (presentation)
     - Logic (business logic)
     - Data (storage)
   - Kjo e bën projektin të vështirë për zgjerim.

2. **Emërtim jo konsistent i variablave dhe funksioneve**
   - Emrat nuk janë gjithmonë deskriptivë
   - Ka mungesë të standardeve (camelCase, meaningful names)

3. **Mungesë e validimit të inputeve**
   - Nuk kontrollohet:
     - input i përdoruesit
     - vlera negative
     - data e pavlefshme
   - Kjo mund të shkaktojë bug-e dhe sjellje të papritura

4. **Error handling pothuajse inekzistent**
   - Nuk ka:
     - try/catch
     - kontroll për null/undefined
     - fallback për gabime

5. **Përdorim i dobët i storage (nëse përdoret)**
   - Nuk ka strukturë të standardizuar për ruajtjen e të dhënave (localStorage ose tjetër)

6. **Kod i duplikuar**
   - Disa funksione ose logjika përsëriten
   - Rrit kompleksitetin dhe vështirëson mirëmbajtjen

7. **Dokumentim minimal ose i munguar**
   - Nuk ka:
     - README të detajuar
     - komentim të kodit
     - shpjegim të arkitekturës

8. **UI Flow jo optimal**
   - Përdoruesi nuk ka feedback të qartë për veprime (p.sh. kur shton në cart)

9. **Siguri bazike e munguar**
   - Nuk ka:
     - validim inputesh
     - mbrojtje ndaj manipulimeve të client-side

---

## 4. 3 përmirësime që do t’i implementoj

### Përmirësimi 1: Refaktorimi i strukturës së kodit

- **Problemi:**
  Kodi është i përzier (UI + logic + data në të njëjtat file)

- **Zgjidhja:**
  Ndarja në:
  - UI Layer
  - Service Layer
  - Data Layer

- **Pse ka rëndësi:**
  - Lehtëson mirëmbajtjen
  - Lejon skalim të sistemit
  - E bën kodin më profesional

---

### Përmirësimi 2: Validim dhe Error Handling

- **Problemi:**
  Inputet nuk kontrollohen dhe mungon trajtimi i gabimeve

- **Zgjidhja:**
  - Validim për inpute
  - Kontroll për null/undefined
  - try/catch për operacione kritike

- **Pse ka rëndësi:**
  - Rrit stabilitetin
  - Parandalon crash-e
  - Përmirëson UX

---

### Përmirësimi 3: Përmirësimi i dokumentimit

- **Problemi:**
  Projekti është i vështirë për t’u kuptuar nga të tjerët

- **Zgjidhja:**
  - README i plotë
  - Komente në kod
  - Strukturë e shpjeguar

- **Pse ka rëndësi:**
  - Lehtëson bashkëpunimin
  - Rrit profesionalizmin
  - Ndihmon onboarding

---

## 5. Një pjesë që ende nuk e kuptoj plotësisht

Një nga pjesët që kërkon kuptim më të thellë është **organizimi i arkitekturës për një sistem më të madh (multi-vendor)**.

Aktualisht projekti është një sistem single-store, dhe kalimi në një arkitekturë ku:

- çdo përdorues ka dyqanin e vet
- të dhënat janë të izoluara
- backend dhe frontend komunikojnë në mënyrë të strukturuar

është një sfidë që kërkon njohuri më të avancuara në:

- system design
- state management
- backend architecture

Kjo është një zonë që dua ta përmirësoj në vazhdim.
