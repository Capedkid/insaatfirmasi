İNŞAAT FİRMASI WEB SİTESİ  
==========================

Bu dosya, projeyi kendi sunucunuzda / bilgisayarınızda çalıştırmanız için hazırlanmıştır.

--------------------------------------------------
1. GEREKLİ YAZILIMLAR
--------------------------------------------------

1) .NET SDK 8.x
   - İndirme adresi: https://dotnet.microsoft.com/en-us/download
   - Kurulumdan sonra komut satırında aşağıyı yazdığınızda sürümü görmelisiniz:
     dotnet --version

2) MySQL Server 8.x (veya uyumlu sürüm)
   - İndirme adresi: https://dev.mysql.com/downloads/mysql/
   - Kurulum sırasında bir root şifresi belirleyin (unutmayın).

3) (İsteğe bağlı) MySQL Workbench
   - Veritabanını görsel arayüz ile yönetmek için kullanabilirsiniz.


--------------------------------------------------
2. MYSQL VERİTABANINI HAZIRLAMA
--------------------------------------------------

1) MySQL’e (Workbench veya komut satırıyla) bağlanın.

2) Aşağıdaki SQL komutlarını çalıştırın:

   CREATE DATABASE insaatfirmasi CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

   CREATE USER 'insaatuser'@'localhost' IDENTIFIED BY 'SIFRENIZI_BURAYA_YAZIN';

   GRANT ALL PRIVILEGES ON insaatfirmasi.* TO 'insaatuser'@'localhost';

   FLUSH PRIVILEGES;

3) Proje klasöründe yer alan appsettings.json dosyasını açın ve 
   ConnectionStrings bölümünü aşağıdaki örneğe göre düzenleyin:

   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Port=3306;Database=insaatfirmasi;User=insaatuser;Password=SIFRENIZI_BURAYA_YAZIN;TreatTinyAsBoolean=true;"
   }

   - Server, Port, Database, User, Password bilgilerini kendi kurulumunuza göre güncelleyin.


--------------------------------------------------
3. TABLOLARI OLUŞTURMA (MİGRATION UYGULAMA)
--------------------------------------------------

1) Komut satırında proje klasörüne geçin:

   cd C:\...yol...\insaat-firmasi

2) Aşağıdaki komutu çalıştırın:

   dotnet ef database update

   - Bu komut, MySQL içinde gerekli tüm tabloları otomatik olarak oluşturur:
     Products, Categories, BlogPosts, Catalogs, Sliders, AboutImages, ContactInfos,
     SiteLogos vb.


--------------------------------------------------
4. UYGULAMAYI ÇALIŞTIRMA
--------------------------------------------------

1) Proje klasöründe şu komutu çalıştırın:

   dotnet run

2) Çıktıda gözüken adresi tarayıcıda açın (genellikle şu adreslerden biri olur):

   https://localhost:5001
   veya
   http://localhost:5000

3) Admin panele giriş:

   Adres:  /Admin

   Varsayılan kullanıcı adı:  admin123
   Varsayılan şifre       :  adminsifre123

   Bu panel üzerinden:
   - Ürünler ve kategoriler
   - Blog yazıları
   - PDF kataloglar
   - Anasayfa slider görselleri ve metinleri
   - Hakkımızda bölümündeki görseller
   - İletişim bilgileri (telefon, e‑posta, adres, WhatsApp)
   - Firma logosu (header logosu)
   gibi içerikleri yönetebilirsiniz.


--------------------------------------------------
5. ADMIN KULLANICI ADI VE ŞİFRESİNİ DEĞİŞTİRME
--------------------------------------------------

Sistem, kolay kurulum için şu anda sabit (hard‑coded) bir admin kullanıcısı ile gelir:

  Kullanıcı adı: admin123
  Şifre       : adminsifre123

Bu bilgileri değiştirmek için iki yol vardır:

--------------------------------------------------
5.1. KOLAY YOL – SABİT KULLANICIYI DEĞİŞTİRME
--------------------------------------------------

1) Projede şu dosyayı açın:

   Controllers\AdminController.cs

2) Aşağıdaki satırları bulun (Login (POST) metodunun içinde):

   if (username == "admin123" && password == "adminsifre123")
   {
       ...
   }

3) Buradaki "admin123" ve "adminsifre123" değerlerini kendi istediğiniz
   kullanıcı adı ve şifre ile değiştirin. Örneğin:

   if (username == "firmaadmin" && password == "GucluSifre!2025")

4) Dosyayı kaydedin ve projeyi yeniden çalıştırın (dotnet run).

   Artık admin panele şu bilgilerle giriş yapabilirsiniz:

   Kullanıcı adı: sizin yazdığınız kullanıcı adı
   Şifre       : sizin yazdığınız şifre


--------------------------------------------------
5.2. GELİŞMİŞ YOL – VERİTABANI TABANLI KULLANICI
--------------------------------------------------

İsterseniz daha ileri seviyede, Users tablosunu kullanarak veritabanı
üzerinden admin kullanıcı oluşturabilir ve sabit kullanıcı bloğunu
devre dışı bırakabilirsiniz. Bu, daha profesyonel bir yöntemdir.

Temel mantık:
  - Users tablosuna IsAdmin = 1 ve IsActive = 1 olan bir kayıt eklemek,
  - AdminController içindeki sabit kullanıcı if bloğunu kaldırmak
    veya yorum satırına almak,
  - Şifre doğrulamasını PasswordHash alanına göre yapmak.

Ancak çoğu senaryo için 5.1’deki “Kolay Yol” yeterli olacaktır.


--------------------------------------------------
6. DİĞER NOTLAR
--------------------------------------------------

- İlk kurulum sonrası:
  - Admin panelinden firma logosu yükleyebilir,
  - İletişim bilgilerinizi kendi telefon, e‑posta ve adresinizle değiştirebilir,
  - Kendi ürünlerinizi, kategorilerinizi, blog yazılarınızı ve kataloglarınızı
    ekleyebilirsiniz.

- Sunucuya yayınlama (hosting) yapacaksanız:
  - Aynı MySQL kurulum ve bağlantı ayarlarını sunucuda da yapmanız gerekir.
  - Ardından dotnet publish ile yayın dosyalarını alıp, sunucu üzerinde
    IIS veya Kestrel + reverse proxy ile çalıştırabilirsiniz.

Sorun yaşamanız durumunda:
- Hata mesajı genelde konsolda veya tarayıcıda detaylı görünür.
- Özellikle “MySqlException” hatalarında, veritabanı bağlantısı ve tablo
  oluşturma adımlarını yeniden kontrol edin.


