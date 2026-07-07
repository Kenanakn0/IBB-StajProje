# Bilet Satış Uygulaması

Etkinlik ve bilet satış platformu. Adminler etkinlik/salon oluşturur, salonun oturmalı/ayakta düzenini tanımlar, kontenjan ve fiyat belirler; kullanıcılar etkinlikleri görüntüleyip bilet satın alır.

> Staj projesi — İstanbul Üniversitesi Bilgi İşlem Daire Başkanlığı.

## Teknolojiler

- **Backend:** .NET Core — katmanlı mimari (API, Application, Domain, Infrastructure)
- **Veritabanı:** EF Core
- **Frontend:** Angular
- **Kimlik doğrulama:** JWT

## Dokümantasyon

- [Proje Planı](docs/proje-plani.md) — genel bakış, mimari, veri modeli, kullanım akışları, çift satış önleme
- [Faz Planı ve Zaman Tahmini](docs/faz-plani.md) — fazlar, görevler ve tahmini süreler

## Kurulum

> Not: Klasör yapısı ve komutlar geliştirme sürecinde netleştirilecektir.

### Backend

```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run
```

### Frontend

```bash
cd frontend
npm install
ng serve
```

## Proje Durumu

Geliştirme fazlara bölünmüştür. Güncel ilerleme ve görev kırılımı için [Faz Planı](docs/faz-plani.md) belgesine bakın.

## Ekip

- [@Kenanakn0](https://github.com/Kenanakn0)
