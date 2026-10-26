# Technical Assesment Project - A .NET MAUI Inventory Management App
Ez a projekt a .NET MAUI tudásom hivatott bemutatni.

## Technikai döntések:
### Entity Framework Core vs Microsoft.Data.Sqlite:
Jelen projekt esetében lehet, hogy az Microsoft.Data.Sqlite (amely egy Microsoft által kínált egyszerű ADO.NET provider SQLite-hoz) választása is elegendőnek bizonyult volna, de végül az EF Core mellett döntöttem (amelyhez az SQLite provider Microsoft.Data.Sqlite-ra épül), mivel az álláshirdetés előnyként említi meg, és ha a későbbiekben továbbfejleszteném ezt a projektet, növelném a komplexitását, akkor EF Core segítségével könnyebben kezelhetőek a táblák közötti relációk, migrációk és egyéb komplexitások.

### Add/Edit FlyoutPage vs ContentPage:
Egy korábbi gyakornoki állásom (illetve a szakdolgozatomban) már fejlesztettem raktárkezelő alkalmazást, amely hasonló volt ehhez.
<img width="528" height="193" alt="image" src="https://github.com/user-attachments/assets/0ab6a8de-b359-46ee-aae5-9ac458fa40e5" />
Itt egy master-detail felületen egy oldalról megjelenő menüben (erről sajnos nincs képem) módosíthatott a felhasználó a cikkek adatain.
Itt is így kezdtem a megoldást, mivel alapvetően szebbnek és használhatóbbnak tartom, viszont mivel a feladat külön oldalt kért (gondolom a navigáció szemléltetéséért), végül eltávolítottam ezt a megoldást, és külön oldalra tettem ezt a felületet, amely a termék sorára való kattintással érhető el.
<img width="1220" height="647" alt="image" src="https://github.com/user-attachments/assets/3cd42770-362c-41cf-8248-498320649882" />
<img width="1220" height="590" alt="image" src="https://github.com/user-attachments/assets/f2949473-51d2-408b-b52f-595637c65efb" />

### Adatfájl elhelyezése:
A .NET MAUI által kínált FileSystem.Current.AppDataDirectory-t szerettem volna használni mindenhol, ám ez Windows alatt AppData/Local/{publisherName}/{packageName}/{productName}/Data, amit különböző packaging opciókkal talán módosítható lett volna, viszont ezt kívül éreztem a projekt scope-jából, úgyhogy az egyszerűség kedvéért Windows alatt külön kezeltem az adatfájlt helyét
'''
string appDataPath = FileSystem.Current.AppDataDirectory;

#if WINDOWS
    appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#endif
'''
Illetve (ez már pláne nem tartozik a scopejába a projektnek, de közben fedeztem fel ezért leírom) nem értettem pontosan, miért a Localra mutat .NET 10 alatt, mert egy korábbi issue és pull request szerint ennek már a Roaming alá kéne mutatnia Local helyett egy .NET 8-as fix óta.
[https://github.com/dotnet/maui/issues/25981](https://github.com/dotnet/maui/issues/25981)
[https://github.com/dotnet/maui/pull/26039](https://github.com/dotnet/maui/pull/26039)
Valószínűleg megtalálnám az okát, ha még fordítanék rá időt, de egyelőre nem találtam meg.

## Amiken még fejlesztenék, ha több idő állna rendelkezésre:
Viszonylag elégedett vagyok a megoldásommal, a felület lehetne szebb és jobban elrendezett, ha több időt fordítanék rá. A feladat követelményeit teljesíti.

## Milyen további fejlesztéseket tennék bele:
### Valuta választás
Egyelőre minden terméknél USD-ben vannak az árak, megoldható lenne, hogy opcióként kiválaszható, milyen valutában lássuk az árakat.
### Lokalizáció
Ha már a valuták válaszhatóak lennének, miért ne lehetne az egész felület például magyarul, vagy akármilyen másik nyelven?
### FlyoutPage
Mivel eltávolítottam, ide beteszem, mert legszívesebben visszatenném.
### Authentication/Authorization
Felhasználók és azok jogköreinek kezelése, ne mindenki tudjon minden terméket megváltoztatni vagy termékeket hozzáadni.
