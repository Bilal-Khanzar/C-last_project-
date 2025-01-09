using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Mail;

class Kisi
{
    protected string isim;
    protected long telefonNumarasi;
    protected string tcKimlik;

    public Kisi(string isim, long telefonNumarasi, string tcKimlik)
    {
        this.isim = isim;
        this.telefonNumarasi = telefonNumarasi;
        this.tcKimlik = tcKimlik;
    }

    public string Isim
    {
        get { return isim; }
        set { isim = value; }
    }

    public long TelefonNumarasi
    {
        get { return telefonNumarasi; }
        set { telefonNumarasi = value; }
    }

    public string TcKimlik
    {
        get { return tcKimlik; }
        set { tcKimlik = value; }
    }

    public virtual void BilgileriGoster()
    {
        Console.WriteLine("İsim: " + isim);
        Console.WriteLine("Telefon Numarası: " + telefonNumarasi);
        Console.WriteLine("T.C. Kimlik Numarası: " + tcKimlik);
    }
}

class Musteri : Kisi
{
    private string istenilenEmlakTuru;

    public Musteri(string isim, long telefonNumarasi, string tcKimlik, string istenilenEmlakTuru)
        : base(isim, telefonNumarasi, tcKimlik)
    {
        this.istenilenEmlakTuru = istenilenEmlakTuru;
    }

    public string IstenilenEmlakTuru
    {
        get { return istenilenEmlakTuru; }
        set { istenilenEmlakTuru = value; }
    }

    public override void BilgileriGoster()
    {
        base.BilgileriGoster();
        Console.WriteLine("İstenen Emlak Türü: " + istenilenEmlakTuru);
    }
}

class Emlakci : Kisi
{
    public Emlakci(string isim, long telefonNumarasi, string tcKimlik)
        : base(isim, telefonNumarasi, tcKimlik) { }

    public override void BilgileriGoster()
    {
        base.BilgileriGoster();
    }

    public bool OdemeYap(double odemeTutari, double emlakFiyati)
    {
        try
        {
            if (odemeTutari >= emlakFiyati)
            {
                Console.WriteLine("Ödeme başarıyla tamamlandı.");
                return true;
            }
            else
            {
                throw new Exception("Ödeme eksik! Maalesef işlem gerçekleştirilemez.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Musteri musteri1 = BilgiAlMusteri();
        Emlakci emlakci1 = BilgiAlEmlakci();

        Console.WriteLine("\nMüşteri Bilgileri:");
        musteri1.BilgileriGoster();
        Console.WriteLine();

        Console.WriteLine("Emlakçı Bilgileri:");
        emlakci1.BilgileriGoster();
        Console.WriteLine();

        double emlakFiyati = 100000;
        while (true)
        {
            try
            {
                Console.Write("Ödeme tutarını giriniz: ");
                double odemeTutari = double.Parse(Console.ReadLine());

                if (emlakci1.OdemeYap(odemeTutari, emlakFiyati))
                {
                    Console.Write("Müşteri sözleşmeyi onaylıyor mu? (E/H): ");
                    string onay = Console.ReadLine().ToUpper();

                    if (onay == "E")
                    {
                        // Müşteri e-posta adresini hardcoded olarak yourdreamstory1@gmail.com olarak değiştiriyoruz
                        string email = "yourdreamstory1@gmail.com";
                        SozlesmeGonder(email, musteri1, emlakci1);
                        Console.WriteLine("Sözleşme e-posta ile gönderildi.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Müşteri işlemi iptal etti. Program sonlandırılıyor.");
                        break;
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Geçersiz ödeme tutarı girdiniz! Lütfen sayısal bir değer giriniz.");
            }
        }
    }

    static Musteri BilgiAlMusteri()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("Müşteri bilgilerini giriniz:");
                Console.Write("İsim ve Soyisim: ");
                string musteriIsim = Console.ReadLine();

                Console.Write("Telefon Numarası: ");
                long musteriTelefon = long.Parse(Console.ReadLine());

                Console.Write("T.C. Kimlik Numarası (11 haneli): ");
                string musteriTc = TCKontrol();

                string musteriEmlakTuru;
                List<string> izinliTurlar = new List<string> { "Daire", "Villa", "Apartman" };
                while (true)
                {
                    Console.Write("İstenen Emlak Türü (Daire, Villa, Apartman):  ");
                    musteriEmlakTuru = Console.ReadLine();
                    if (izinliTurlar.Contains(musteriEmlakTuru, StringComparer.OrdinalIgnoreCase))
                    {
                        musteriEmlakTuru = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(musteriEmlakTuru.ToLower());
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Hatalı giriş! Lütfen sadece 'Daire', 'Villa' veya 'Apartman' yazın.");
                    }
                }

                return new Musteri(musteriIsim, musteriTelefon, musteriTc, musteriEmlakTuru);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    static Emlakci BilgiAlEmlakci()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("\nEmlakçı bilgilerini giriniz:");
                Console.Write("İsim ve Soyisim: ");
                string emlakciIsim = Console.ReadLine();

                Console.Write("Telefon Numarası: ");
                long emlakciTelefon = long.Parse(Console.ReadLine());

                Console.Write("T.C. Kimlik Numarası (11 haneli): ");
                string emlakciTc = TCKontrol();

                return new Emlakci(emlakciIsim, emlakciTelefon, emlakciTc);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    static string TCKontrol()
    {
        while (true)
        {
            string tc = Console.ReadLine();
            if (tc.Length == 11 && long.TryParse(tc, out _))
            {
                return tc;
            }
            else
            {
                Console.WriteLine("Geçersiz T.C. Kimlik Numarası! Lütfen 11 haneli bir numara giriniz.");
            }
        }
    }

    static void SozlesmeGonder(string email, Musteri musteri, Emlakci emlakci)
    {
        string konu = "Emlak Sözleşmesi";
        string icerik = $"Sözleşme Detayları: sozlesmeyi kabul ettiginiz icin tesekkur ederiz\n" +
            $" sizinde bu daireyapim projeyi cok yakinda baslayacagiz sartlarimiz kabul edilmistir ve bu projeyi siz onay verdiniz  \n\n" +
                        $"Müşteri Bilgileri:\n" +
                        $"İsim: {musteri.Isim}\nTelefon: {musteri.TelefonNumarasi}\nT.C. Kimlik: {musteri.TcKimlik}\nEmlak Türü: {musteri.IstenilenEmlakTuru}\n\n" +
                        $"Emlakçı Bilgileri:\n" +
                        $"İsim: {emlakci.Isim}\nTelefon: {emlakci.TelefonNumarasi}\nT.C. Kimlik: {emlakci.TcKimlik}\n";

        try
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("bilalkhanzer985@gmail.com", "tikm lxrj ahns jnzk"),
                EnableSsl = true
            };

            MailMessage mail = new MailMessage
            {
                From = new MailAddress("bilalkhanzer985@gmail.com"),
                Subject = konu,
                Body = icerik
            };
            mail.To.Add(email);

            smtpClient.Send(mail);
        }
        catch (Exception ex)
        {
            Console.WriteLine("E-posta gönderiminde bir hata oluştu: " + ex.Message);
        }
    }
}
