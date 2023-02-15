# BrainiacQuest-MageGames
 
NOTLAR


Json dosyasında tekrar eden veriler var. Değişiklik yapmadım, aynı şekilde kullandım.


Leaderboard'da infinite scroll kolay görülebilsin diye Scrollview küçük halde kullandım.


Geliştirilebilir halde olabilmesi açısından bazı yerlerde esnek yazmak istedim.


AudioManager benim yazdığım bir paket, küçük işlerde gayet kullanışlı. Pool ediyor ve gereksiz AudioSource oluşturmuyor, Lazy Instantiation ile Object Pooling kullandım.


Gerekli yerlerde Singleton uyguladım.


Preset'ler atayarak, ayarları tekrar tekrar manuel yapmamaya dikkat ettim.


UI Assetleri için Sprite Atlas ve Tek bir Material (Geometry 2000) olarak kullandım ki Batch Count yükselmesin. Sadece bazı hareketli öğeler birkaç Batch sayısı arttırıyor, onlar haricinde (gözden kaçırdığım bir şey yoksa) batch 3'ü geçmiyor.


Paket olarak, kendi yazdığım AudioManager haricinde, çok kullanışlı olan UI Extensions paketini dahil ettim.


Eskiden UI animasyonları için yazdığım Tween sciptlerini burada da kullandım.


Scriptable Object bilgimi de gösterebilmek için, SO kullandım. Zaten gerekliydi, sadece bildiğimi göstermek için kullanmadım :)


Birkaç farklı font denedim ama genellikle birkaç Türkçe karakter eksiği oluyor genellikle. En son kullandığım font için Extended ASCII olarak bir Font Asset yarattım ve bir de Fallback Asseti ekledim (her dil için ayrı bir Fallback yaratmak gerekiyor.)
