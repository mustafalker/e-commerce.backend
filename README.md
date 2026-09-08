# E-Commerce Backend API Architecture

Bu proje, modern yazılım geliştirme standartları, **Nesne Yönelimli Programlama (OOP)** prensipleri ve **Clean Architecture** (Temiz Mimari) kuralları uygulanarak geliştirilmiş bir E-Ticaret backend sistemidir. Öğrenme, pratik yapma ve modern mimari yaklaşımları uygulama amacıyla geliştirilmiştir.

---

## 1. Clean Architecture ve Katmanların Sorumlulukları

Proje 4 ana katmandan oluşmaktadır ve Dependency Rule (Bağımlılık Kuralı) gereği dıştan içe doğru bir bağımlılık hiyerarşisine sahiptir.

*   **`Domain` (Çekirdek Katman):** Hiçbir kütüphaneye veya framework'e (EF Core, ASP.NET vs.) bağımlı değildir. Saf C# (POCO) nesnelerinden oluşur. Sistemin **İş Kurallarını (Business Rules)** ve temel yapıtaşlarını (`Order`, `User`) barındırır.
*   **`Application` (Uygulama Katmanı):** Sistemin "Kullanım Senaryolarını" (Use Cases) tanımlar. DTO'lar (Data Transfer Objects), Interface'ler (`IOrderRepository`, `IPasswordHasher`), iş servisleri (`CheckoutService`) ve ödeme stratejileri bu katmandadır. Veritabanının "ne" olduğu ile ilgilenmez, tamamen soyutlanmıştır.
*   **`Infrastructure` (Altyapı Katmanı):** Application katmanında tanımlanan "Soyutlamaların" somut (concrete) uygulamalarını barındırır. Entity Framework Core (`AppDbContext`), PostgreSQL bağlantıları, BCrypt işlemleri (`PasswordHasher`), Dapper sorguları ve veritabanı yansımaları (`OrderEntity`, `UserEntity`) bu katmandadır.
*   **`API` (Sunum Katmanı):** Dış dünyanın sistemle iletişim kurduğu HTTP uç noktalarıdır (Controllers). Gelen HTTP isteklerini alır ve doğrudan iş mantığına girmeden Application katmanındaki servislere iletir.

---

## 2. OOP Prensipleri ve Uygulama Alanları

### A. Encapsulation (Kapsülleme)
Bir nesnenin iç durumunu (state) dışarıdan rastgele değiştirilmeye karşı korumak ve değişimi kontrollü bir şekilde yapmaktır.
*   **Domain Katmanında:** `Order` ve `User` entity'lerinin property setter'ları `private set;` olarak tanımlanmıştır. Bu nesneler ancak Constructor (Yapıcı Metot) aracılığıyla ve iş kuralları (validasyonlar) işletildikten sonra "geçerli" (valid) bir state ile oluşturulabilir. Durum değişiklikleri ise sadece `UpdatePassword` gibi kontrollü davranış metotlarıyla yapılır.

### B. Abstraction (Soyutlama) ve Inversion of Control
Sistemin karmaşıklığını interface'ler (arayüzler) arkasına saklayarak sınıflar arasındaki sıkı bağı (tight coupling) kopardık.
*   **Örnekler:** `IPaymentMethod`, Kredi Kartı veya Kapıda Ödeme gibi sistemlerin detaylarını API katmanından gizler. `IUserRepository` arayüzü sayesinde Application katmanı verilerin PostgreSQL'de tutulduğunu bilmez, sadece sözleşmeye (contract) uyar.

### C. Polymorphism (Çok Biçimlilik) ve Tasarım Desenleri
*   **Strategy Pattern (Strateji Deseni):** Ödeme sırasında `if-else` blokları kullanmak yerine, her ödeme yöntemi için (`CreditCardPayment`, `MealCardPayment`, `CashOnDeliveryPayment`) ayrı sınıflar oluşturulmuştur. Hepsi `IPaymentMethod` interfacesinden türer ve Polimorfizm sayesinde çalışma zamanında doğru ödeme işlemi çalıştırılır.
*   **Factory Pattern (Fabrika Deseni):** `PaymentFactory` sınıfı ile hangi ödeme stratejisinin örnekleneceği (instantiation) kararını Controller'dan uzaklaştırdık. Bu, **Open/Closed Principle (Açık/Kapalı Prensibi)**'ni destekler; sisteme yeni bir ödeme türü geldiğinde mevcut sınıflar değiştirilmez, sadece yeni bir strateji sınıfı eklenir.

---

## 3. Veri Aktarımı ve Dönüşümler (DTO & AutoMapper)

*   **DTO (Data Transfer Object):** Kullanıcıdan alınan HTTP istek verisi ile iş modelimiz birbirinden farklıdır. Over-posting zafiyetlerini önlemek ve Single Responsibility (Tek Sorumluluk) prensibine uymak için `Application/DTOs` klasöründe veri taşıyan sınıflar oluşturulmuştur.
*   **AutoMapper:** Infrastructure katmanında, Domain'deki iş nesnelerini veritabanı nesnelerine (`OrderEntity`, `UserEntity`) dönüştürmek için manuel eşleme yerine AutoMapper kullanılmıştır. Bu işlem `InfrastructureMappingProfile` üzerinde konfigüre edilmiştir.

---

## 4. Performans ve Ölçeklenebilirlik: CQRS Yaklaşımı ve Dapper

Sistemin okuma ve yazma gereksinimleri birbirinden farklı olduğu için yapı **CQRS (Command Query Responsibility Segregation)** temel prensiplerinden ilham alınarak tasarlanmıştır.

*   **Yazma (Write/Command) Operasyonları:** Sipariş oluşturma, kullanıcı kaydetme gibi işlemler iş kuralı (business logic) ve tutarlılık gerektirdiğinden **Entity Framework Core** ve Repository pattern kullanılarak gerçekleştirilmiştir.
*   **Okuma (Read/Query) Operasyonları:** Sipariş listeleme gibi okuma işlemlerinde EF Core'un tracking (takip) maliyetine katlanmamak adına `OrderQueryService` içinde **Dapper** (Micro-ORM) kullanılmıştır. Dümdüz SQL atılarak maksimum performans sağlanmıştır.

---

## Kurulum ve Çalıştırma

1.  Bilgisayarınızda **Docker**, **.NET SDK** (sürüm 10 önerilir) bulunduğundan emin olun.
2.  PostgreSQL veritabanını ayağa kaldırmak için ana dizinde terminal açıp şu komutu çalıştırın:
    ```bash
    docker-compose up -d
    ```
3.  Uygulamayı derleyin ve çalıştırın:
    ```bash
    cd [ProjeDizini].API
    dotnet run
    ```
4.  Proje başarıyla derlendikten sonra tarayıcınız üzerinden **Swagger UI** otomatik olarak açılacaktır. JWT yetkilendirmesi Swagger'a entegre edilmiştir. `Register` endpoint'i ile kullanıcı oluşturup `Login` endpoint'inden alacağınız JWT token'ını "Authorize" butonuna basarak girebilir ve korumalı kaynaklara erişebilirsiniz.
