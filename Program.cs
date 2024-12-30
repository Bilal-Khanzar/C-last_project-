using System;
using System.Collections.Generic;

// Ortak Kişi sınıfı
class Kisi 
{
    protected string isim;
    protected long telefonNumarasi; // Telefon numarası long olarak alındı

    public Kisi(string isim, long telefonNumarasi)
    {
        this.isim = isim;
        this.telefonNumarasi = telefonNumarasi;
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

    public virtual void BilgileriGoster()
    {
        Console.WriteLine("İsim: " + isim);
        Console.WriteLine("Telefon Numarası: " + telefonNumarasi);
    }
}

// Müşteri sınıfı
class Musteri : Kisi
{
    private string istenilenEmlakTuru;

    public Musteri(string isim, long telefonNumarasi, string istenilenEmlakTuru) : base(isim, telefonNumarasi)
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

// Emlakçı sınıfı
class Emlakci : Kisi
{
    public Emlakci(string isim, long telefonNumarasi) : base(isim, telefonNumarasi) { }

    public override void BilgileriGoster()
    {
        base.BilgileriGoster();
    }

    public void EmlakSun(Musteri musteri)
    {
        Console.WriteLine(musteri.Isim + " için " + musteri.IstenilenEmlakTuru + " arayışında yardımcı oluyorum.");
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

    public string EmlakSecimYap()
    {
        List<string> secenekler = new List<string> { "Daire", "Villa", "Apartman" };
        while (true)
        {
            try
            {
                Console.WriteLine("Lütfen seçmek istediğiniz emlak türünü seçin: ");
                // musterinin emlak secim kullanma hakki 
                for (int i = 0; i < secenekler.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + secenekler[i]);
                }

                int secim = int.Parse(Console.ReadLine());
                if (secim > 0 && secim <= secenekler.Count)
                {
                    return secenekler[secim - 1]; // girilen degerlerimin bir eksili alinir 
                }
                else
                {
                    throw new Exception("Yanlış tuşladınız! Lütfen 1 ile 3 arasında bir seçim yapın.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);// burada ki massage throw da mesaji ekrar karsima cikartacak 
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {

        
        Musteri musteri1 = BilgiAlMusteri(); // Polimorfizm (Polymorphism): oluyor 
        Emlakci emlakci1 = BilgiAlEmlakci();
        
        Console.WriteLine("\nMüşteri Bilgileri:");
        musteri1.BilgileriGoster();
        Console.WriteLine();

        Console.WriteLine("Emlakçı Bilgileri:");
        emlakci1.BilgileriGoster();
        Console.WriteLine();

        string secilenEmlak = emlakci1.EmlakSecimYap();// 3 tane emlak turunde bir tanesini sececek 
        musteri1.IstenilenEmlakTuru = secilenEmlak;

        Console.WriteLine();
        emlakci1.EmlakSun(musteri1);

        double emlakFiyati = 100000;
        while (true)
        {
            try
            {
                Console.Write("Ödeme tutarını giriniz: ");
                double odemeTutari = double.Parse(Console.ReadLine());

                if (emlakci1.OdemeYap(odemeTutari, emlakFiyati))
                {
                    Console.WriteLine("Emlak işlemi başarıyla tamamlandı.");
                    break;
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
                if (string.IsNullOrWhiteSpace(musteriIsim) || IsNumeric(musteriIsim)) // burada ki string IsNullOrWhiteSpace metodu, bir string ifadenin boşluk karakterlerinden oluşup oluşmadığını
                                                                                      // veya tamamen boş olup olmadığını kontrol etmek için kullanılır.
                {
                    throw new Exception("İsim sadece harflerden oluşmalıdır!");
                }

                long musteriTelefon;
                Console.Write("Telefon Numarası (Sadece sayısal bir değer giriniz): ");
              //Diyelim ki bir kullanıcının girdiği metni bir sayıya dönüştürmek istiyorsunuz. Ancak, kullanıcı geçerli
                                                                           //bir sayıya girmeyebilir (örneğin, bir harf veya başka geçersiz karakterler). İşte burada TryParse devreye girer.
                                                                       //Eğer dönüşüm başarılıysa, sonucu elde edersiniz; başarısız olursa hata almazsınız, sadece false döne
                    
                if (!long.TryParse(Console.ReadLine(), out musteriTelefon))
                        {
                    throw new Exception("Telefon numarası sadece sayısal olmalıdır!");
                }

                Console.Write("İstenen Emlak Türü (örn. Daire): ");
                string musteriEmlakTuru = Console.ReadLine();

                return new Musteri(musteriIsim, musteriTelefon, musteriEmlakTuru);
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
                if (string.IsNullOrWhiteSpace(emlakciIsim) || IsNumeric(emlakciIsim))
                {
                    throw new Exception("İsim sadece harflerden oluşmalıdır!");
                }

                long emlakciTelefon;
                Console.Write("Telefon Numarası (Sadece sayısal bir değer giriniz): ");
                if (!long.TryParse(Console.ReadLine(), out emlakciTelefon))
                {
                    throw new Exception("Telefon numarası sadece sayısal olmalıdır!");
                }

                return new Emlakci(emlakciIsim, emlakciTelefon);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    static bool IsNumeric(string input)
    {
        foreach (char c in input)
        {
            if (char.IsDigit(c)) return true;
        }
        return false;
    }
}
 