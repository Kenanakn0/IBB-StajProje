# Proje Planı — Bilet Satış Uygulaması

Backend: .NET Core (katmanlı mimari) · Frontend: Angular

## 1. Genel Bakış

Bir etkinlik ve bilet satış platformu geliştirilecek. Adminler etkinlik oluşturacak, salonun oturmalı/ayakta düzenini tanımlayacak, kontenjan ve fiyat belirleyecek. Kullanıcılar ise etkinlikleri görüntüleyip bilet satın alacak.

Backend .NET Core ile katmanlı mimari kullanılarak, frontend Angular ile geliştirilecek.

## 2. Mimari Şema

Proje 4 katmandan oluşacak. Her katman yalnızca kendi işine bakacak; katmanlar arasındaki bağımlılık tek yönlü olacak. Böylece bir katmanda yapılan değişiklik diğerlerini bozmayacak.

| Katman | Ne işe yarayacak |
|---|---|
| **API** | Angular'dan gelen istekleri karşılar, ilgili işlemi Application katmanına yönlendirir. |
| **Application** | İş kuralları burada olur (kontenjan kontrolü, rezervasyon süresi, indirim hesabı gibi). |
| **Domain** | Etkinlik, Bilet, Kullanıcı gibi temel varlıkların tanımlandığı yer. Başka hiçbir katmana bağımlı değildir. |
| **Infrastructure** | Veritabanı işlemleri burada olur (EF Core ile). |

**Veri akışı:** Angular → API → Application → Infrastructure (veritabanı) → sonuç aynı yoldan geri döner.

## 3. Veri Modeli

| Varlık | Ne tutacak | Bağlantısı |
|---|---|---|
| **Kullanici** | Ad, e-posta, şifre, rol (Admin / Kullanıcı) | — |
| **Salon** | Salonun adı, adresi | 1 Salon → birden çok Etkinlik |
| **Bolum** | VIP / Standart / Ayakta gibi alan bilgisi; oturmalı mı değil mi | 1 Salon → birden çok Bölüm |
| **Etkinlik** | İsim, tarih, açıklama | 1 Salon'a bağlı |
| **EtkinlikBolum** | Bir etkinlikteki bir bölümün fiyatı ve kontenjanı | Etkinlik + Bölüm birleşimi |
| **Koltuk** | Blok / sıra / koltuk no (sadece oturmalı bölümlerde) | Bölüm'e bağlı |
| **Rezervasyon** | 5 dakikalık geçici tutma kaydı (satın almadan önce) | Kullanıcı + EtkinlikBölüm + (varsa) Koltuk |
| **Bilet** | Ödemesi tamamlanmış bilet bilgisi | Kullanıcı + EtkinlikBölüm + (varsa) Koltuk |
| **Indirim** | Öğrenci indirimi gibi indirim kuralı | Satın alma sırasında biletin fiyatına uygulanır |

**Önemli kararlar:**

- Admin ve Kullanıcı aynı tabloda tutulacak; aradaki fark bir "rol" bilgisiyle ayrılacak.
- Bilet tek tablo olacak; koltuk bilgisi yalnızca oturmalı biletlerde dolu olacak, ayakta biletlerde boş kalacak.
- Fiyat ve kontenjan bilgisi Bölüm'de değil EtkinlikBölüm'de tutulacak, çünkü aynı bölüm farklı etkinliklerde farklı fiyatlanabilir.
- Rezervasyon ve Bilet ayrı varlıklar: Rezervasyon geçici (5 dk) tutma kaydıdır, ödeme başarılı olunca ona karşılık bir Bilet oluşur.

## 4. Ana Kullanım Akışları

### 4.1 Admin — Etkinlik Oluşturma

1. Admin giriş yapar.
2. Etkinlik bilgilerini girer (isim, tarih, açıklama).
3. Bir salon seçer.
4. Salonun bölümleri için fiyat ve kontenjan belirler (EtkinlikBölüm).
5. Kaydeder; etkinlik kullanıcılara görünür hale gelir.

### 4.2 Kullanıcı — Bilet Satın Alma

1. Kullanıcı etkinlikleri listeler ve birini seçer.
2. Bir bölüm seçer (oturmalıysa koltuk, ayaktaysa adet belirler).
3. Satın al'a basar; sistem koltuğu/kontenjanı 5 dakikalığına geçici olarak ayırır (Rezervasyon).
4. Varsa indirim uygulanır.
5. Ödeme adımı tamamlanır (gerçek değil, simülasyon).
6. Ödeme başarılıysa bilet oluşur; 5 dakika içinde tamamlanmazsa geçici ayırma iptal olur.

## 5. Çift Satış Önleme ve Geçici Kontenjan Tutma

Projenin en kritik teknik problemi budur: iki kişi aynı anda aynı koltuğu almaya çalışırsa yalnızca birine izin verilmesi garanti altına alınmalı. Bunun için üç mekanizma birlikte kullanılacak:

- **Unique constraint:** Veritabanında aynı koltuğun iki kez satılamayacağı bir kural konur — bu, son ve kesin güvencedir.
- **Optimistic concurrency:** Bir kayıt güncellenirken o kayıt başka biri tarafından değiştirilmişse işlem hata verir ve tekrar denenmesi istenir.
- **Süreli rezervasyon:** Rezervasyonlara 5 dakikalık son geçerlilik tarihi eklenir; süresi dolanlar arka planda çalışan bir görevle otomatik temizlenir.
