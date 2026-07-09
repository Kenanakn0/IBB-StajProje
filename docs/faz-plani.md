# Faz Planı ve Zaman Tahmini

Fazlar 1'den 4'e sıralı ilerler; bir faz büyük ölçüde bitmeden diğerine geçilmez. Sıralamanın mantığı: önce üzerinde çalışılacak veri (etkinlik, salon, koltuk) sistemde var olmalı, sonra bu veri üzerinden satın alma ve çift satış önleme kurulmalı, en son arayüz gelmeli.

## Özet

| Faz | Adı | Amaç | Tahmini süre |
|---|---|---|---|
| Faz 1 | Proje İskeleti ve Veri Modeli | Katmanlı yapıyı, entity'leri ve veritabanını hazırlamak. | 21 saat |
| Faz 2 | Kimlik Doğrulama + Admin CRUD | Giriş yapılabilmesi ve satılacak etkinlik/salon/koltukların oluşturulabilmesi. | 27 saat |
| Faz 3 | Bilet Satın Alma + Çift Satış Önleme | Bilet alımı ve aynı koltuğun iki kez satılmasının engellenmesi (en kritik). | 24 saat |
| Faz 4 | Angular Arayüzü | Backend'i kullanan kullanıcı ve admin ekranları. | 18 saat |

**Toplam tahmin:** 90 saat · **Çalışma kapasitesi:** ~90 saat (6 sa/gün × 15 gün, ~3 hafta)

> **Risk notu:** Toplam tahmin mevcut kapasiteye çok yakın olduğu için planda tampon (buffer) süre yok. Yazılımda tahminler pratikte genellikle aşıldığından, ilerleyen günlerde gerçekleşen süreler tahminlerle karşılaştırılacak; sapma olursa öncelik en kritik fazlara (Faz 2 ve Faz 3) verilip Faz 4'teki opsiyonel işler sadeleştirilecek.

---

## Faz 1 — Proje İskeleti ve Veri Modeli (Kurulum) · 21 saat

**Amaç:** Kod yazmaya başlayabilmek için altyapıyı hazırlamak. Bu fazın sonunda henüz iş kuralı yok; derlenen ve veritabanına bağlanan boş bir proje var.

| Görev | Tahmini |
|---|---|
| 4 katmanlı solution + katman referansları | 6 sa |
| Domain entity'leri (9 adet) ve ilişkiler | 5 sa |
| EF Core DbContext + connection string | 5 sa |
| İlk migration + FK/ilişki kontrolü | 4 sa |
| GitHub repo + .gitignore + yetkilendirme + ilk commit | 1 sa |

**Bitiş kriteri:** Proje derleniyor, veritabanına bağlanıyor ve tüm tablolar oluşmuş durumda.

## Faz 2 — Kimlik Doğrulama ve Admin Yönetim (CRUD) · 27 saat

**Amaç:** Sisteme giriş yapılabilmesi ve satılacak etkinliklerin admin tarafından oluşturulabilmesi. (Etkinlik/salon oluşturma CRUD'u yalnızca bu fazdadır.)

| Görev | Tahmini |
|---|---|
| Kullanıcı kaydı/girişi + şifre hash | 3 sa |
| JWT üretimi ve doğrulaması | 4 sa |
| Rol bazlı yetkilendirme | 3 sa |
| Salon CRUD | 3 sa |
| Bölüm CRUD | 3 sa |
| Etkinlik CRUD | 3 sa |
| EtkinlikBölüm (fiyat/kontenjan) | 4 sa |
| Koltuk oluşturma | 4 sa |

**Bitiş kriteri:** Admin giriş yapıp bir etkinliği, salonu ve bölümlerini fiyat/kontenjanıyla oluşturabiliyor. Artık sistemde "satılabilecek" veri var.

## Faz 3 — Bilet Satın Alma ve Çift Satış Önleme (En Kritik Faz) · 24 saat

**Amaç:** Kullanıcının bilet alması ve aynı koltuğun iki kez satılmasının kesin olarak engellenmesi.

| Görev | Tahmini |
|---|---|
| Etkinlik listeleme + detay uçları | 4 sa |
| Bölüm/koltuk seçimi | 2 sa |
| 5 dakikalık geçici rezervasyon (hold) | 2,5 sa |
| Unique constraint | 2,5 sa |
| Optimistic concurrency (RowVersion) | 3 sa |
| Arka plan temizleme görevi (BackgroundService) | 2 sa |
| İndirim hesaplama (interface ile) | 3 sa |
| Ödeme simülasyonu + Rezervasyon→Bilet | 5 sa |

**Bitiş kriteri:** İki kullanıcı aynı koltuğu almaya çalıştığında yalnızca biri başarılı oluyor; ödeme sonrası bilet oluşuyor.

## Faz 4 — Angular Arayüzü · 18 saat

**Amaç:** Faz 2 ve Faz 3'te yazılan API'leri kullanan kullanıcı ve admin ekranlarını yapmak. (Yeni backend/CRUD yazılmaz, mevcut API'ler tüketilir.)

| Görev | Tahmini |
|---|---|
| Angular kurulum + routing + HTTP + JWT interceptor | 6 sa |
| Kullanıcı ekranları (liste, detay, seçim, satın alma) | 5 sa |
| Admin ekranları (salon/bölüm/etkinlik yönetimi) | 3 sa |
| Rezervasyon geri sayımı (opsiyonel) | 4 sa |

**Bitiş kriteri:** Uçtan uca çalışan arayüz: admin etkinlik oluşturuyor, kullanıcı görüp bilet alıyor.

---

## Tahmin Takibi

Tahmin becerisini geliştirmek için her fazı bitirdikçe **gerçekleşen süreyi** aşağıya not et; tahmin ile gerçek arasındaki farkı görmek bir sonraki tahminini isabetli yapar.

| Faz | Tahmini | Gerçekleşen | Fark |
|---|---|---|---|
| Faz 1 | 21 sa | 11 sa | 10 sa |
| Faz 2 | 27 sa | | |
| Faz 3 | 24 sa | | |
| Faz 4 | 18 sa | | |
| **Toplam** | **90 sa** | | |
