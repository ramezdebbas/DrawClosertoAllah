using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Step Points",
                    "Step Points",
                    "Assets/Images/10.jpg",
                    "It should be the desire of every Muslim to draw nearer and closer to Almighty Allah the most compassionate and the most merciful. By drawing closer and near to Allah we gain his help in every aspect of our lives in this world and to be merciful to us so that we may gain mercy in the hereafter in order to enter Jannah in the next world.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Fulfilling obligatory duties and abstaining from prohibited matters",
                    "The Prophet (May Allah bless him and grant him peace) has guided us. And Allah, the Glorified and the Exalted, has guided us how to draw near to Him. As He said According to Hadith Qudsi.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nThe Prophet (May Allah bless him and grant him peace) has guided us. And Allah, the Glorified and the Exalted, has guided us how to draw near to Him. As He said According to Hadith Qudsi:\n\n“Whoever draws near to Me among those drawn near by fulfilling what I have made obligatory on them…. You do not draw near to Allah except by fulfilling the obligatory duties which Allah has made obligatory on you; (that is) the obligatory duties from the obligatory duties (Faraid) such as Prayers, and Zakat (obligatory charity), and Hajj, and Fasting and being good to Parents and all these obligatory duties on you draw you near to Allah, the Glorified and the Exalted. And the faraid (obligatory duties) are the first things that draw you near to Allah. You do not reach the door of nearness nor do you reach to the presence of nearness except by fulfilling the obligatory duties. This is the first thing that draws you near to Allah, the Glorified and the Exalted, and you are in His Presence. So the first thing we need to do is fulfil ALL of our obligatory duties and abstain from that which Allah and his messenger have forbidden us from.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Step Points", CreatedOnTwo = "Item", CreatedTxtTwo = "Fulfilling obligatory duties and abstaining from prohibited matters", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Draw Closer to ALLAH" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Nawafil Prayers",
                     "My slave keeps on coming closer to Me through performing Nawafil (voluntary deeds) until I love him, so I become his sense of hearing with which he hears, and his sense of sight with which he sees, and his hand with which he grips, and his leg with which he walks; and if he asks Me, I will give him, and if he asks My Protection.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nHow can we do Zikr all the time whilst we have so many things to do in life such as we go to work, feed the family, pay bills, etc.? Well, the answer to this is that, firstly, it is the Sunnah of Allah's Messenger to work and feed the family which is rewarded by Allah and, secondly, that when a person is working or at school or wherever a person may be this does not mean that he should stop remembering Allah while he is working, while he is serving his customers or while he is programming his computer. In fact the heart of the believer should be attached to Allah's remembrance throughout his day to day activities such that he protects himself from falling in love with the materials and temptations of this world and that his love for Allah and his Messenger becomes stronger and stronger as time goes by. When a person begins to remember Allah all the time it is then that he becomes conscious of his actions.Remember: in a place where people are oblivious to dhikir, remembrance of Allah is like being steadfast in jihad, when others are running away.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Step Points", CreatedOnTwo = "Item", CreatedTxtTwo = "Nawafil Prayers", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Draw Closer to ALLAH" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Zikr",
                     "Verily, in the remembrance of Allah do hearts find rest. (13:28) Remembrance of Allah indeed is the greatest virtue.(29:46). O ye who believe, remember Allah much. And glorify Him morning and evening (33:42-43)",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\n …Verily, in the remembrance of Allah do hearts find rest. (13:28) Remembrance of Allah indeed is the greatest virtue.(29:46). O ye who believe, remember Allah much. And glorify Him morning and evening (33:42-43) Remembrance of Allah is the foundation of all good deeds. Whoever succeeds in it is blessed with the close friendship of Allah. That is why the Prophet, peace be upon him, used to make remembrance of Allah at all times. When a man complained, The laws of Islam are too heavy for me, so tell me something that I can easily follow, the Prophet told him, Let your tongue be always busy with the remembrance of Allah. [Ahmad]. Remembrance of Allah the best of deeds The Prophet, peace be upon him, would often tell his Companions, Shall I tell you about the best of deeds, the most pure in the sight of your Lord, about the one that is of the highest order and is far better for you than spending gold and silver, even better for you than meeting your enemies in the battlefield where you strike at their necks and they at yours? The Companions replied, Yes, O Messenger of Allah! The Prophet, peace be upon him, said, Remembrance of Allah. (Tirmidhi,Ahmad) Abu Musa Al-Ash`ari (May Allah be pleased with him) reported: The Prophet (PBUH) said, The similitude of one who remembers his Rubb and one who does not remember Him, is like that of the living and the dead.'' [Al-Bukhari and Muslim]. Zikr can be done anywhere and anytime. \n\nA person may ask, ‘How can we do Zikr all the time whilst we have so many things to do in life such as we go to work, feed the family, pay bills, etc.? Well, the answer to this is that, firstly, it is the Sunnah of Allah's Messenger to work and feed the family which is rewarded by Allah and, secondly, that when a person is working or at school or wherever a person may be this does not mean that he should stop remembering Allah while he is working, while he is serving his customers or while he is programming his computer. In fact the heart of the believer should be attached to Allah's remembrance throughout his day to day activities such that he protects himself from falling in love with the materials and temptations of this world and that his love for Allah and his Messenger becomes stronger and stronger as time goes by. When a person begins to remember Allah all the time it is then that he becomes conscious of his actions.Remember: in a place where people are oblivious to dhikir, remembrance of Allah is like being steadfast in jihad, when others are running away. (Targhib, p. 193, vol. 3 ref. Bazar and Tibrani)... And the men and the women who remember Allah much with their hearts and tongues. Allah has prepared for them forgiveness and a great reward (i.e., Jannah).'' (33:35)",
                     group1) { CreatedOn = "Group", CreatedTxt = "Step Points", CreatedOnTwo = "Item", CreatedTxtTwo = "Zikr", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Draw Closer to ALLAH" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Calling upon Allah in Dua (supplication)",
                     "Allah the most compassionate says in the Holy Quran, Call on Me. I will answer your prayer, but those who are too arrogant to serve me will surely find themselves humiliated in Hell (40:60). Allah the Exalted, has said: And your Lord says: Pray unto me: and I will hear your prayer (Quran 40:60)",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAllah the most compassionate says in the Holy Quran, Call on Me. I will answer your prayer, but those who are too arrogant to serve me will surely find themselves humiliated in Hell (40:60). Allah the Exalted, has said: And your Lord says: Pray unto me: and I will hear your prayer (Quran 40:60), Call upon your Lord Humbly and in secret (Quran 7:55), When My servants question thee concerning Me, I am indeed close (to them): I listen to the prayer of every suppliant when he calleth on Me (Quran 2:186), Is not He (best) who listens to the (soul) distressed when it calls on Him, and who relieves its suffering. (Quran 27:62) Dua’s are never wasted Aisha radhiallaahu anha said, No believer makes Dua and it is wasted. Either it is granted here in this world or deposited for him in the Hereafter as long as he does not get frustrated. Allah’s anger at those who don’t make dua In fact, it is even wrong to never make Dua, Whosoever does not supplicate to Allah, He will be angry with Him. [Saheeh Jaami`as-Sagheer #2414] Dua is a weapon for the believers Rasullullah is reported to have said, “Dua is the weapon of a Muslim”.\n\nDua for ones brother in his absence. The supplication that gets the quickest answer is the one made by one Muslim for another in his absence. [Abu Daw'ud and Tirmidhi] So let us build a close relationship with Allah by making much dua to him. He loves it when his slave calls upon him and it angers him if his slave does not call upn him. Let us have full hope that our dua’s will be accepted and if you think they won’t they know Allah is keeping the rewards for you in the hereafter and those rewards are so great that one would wish that none of there duas were excepted in this world just so that one can gain all the rewards for their duas in the next world.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Step Points", CreatedOnTwo = "Item", CreatedTxtTwo = "Calling upon Allah in Dua (supplication)", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Draw Closer to ALLAH" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "Building a close relationship with the Qur’an",
                     "Recite the Holy Qur’aan as much as we can for It will come as an intercessor for its reciter’ on the Day of Judgement [Muslim] Learn the Qur’an and recite it, because the example of one who learns the Qur’an, reads it and recites it in Tahajjud is like an open bag full of musk whose fragrance permeates the entire place.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nRecite the Holy Qur’aan as much as we can for It will come as an intercessor for its reciter’ on the Day of Judgement [Muslim] Learn the Qur’an and recite it, because the example of one who learns the Qur’an, reads it and recites it in Tahajjud is like an open bag full of musk whose fragrance permeates the entire place. And the person who has learnt the Qur’an but sleeps while the Qur’an is in the heart is like a bag full of musk but with its mouth closed. Virtues of reciting the Qur’an\n\n“Whoever reads a letter from the Book of Allah will receive a hasanah (good deed) from it (i.e. his recitation), and the hasanah is multiplied by ten. I do not say that Alif-Laam-Meem is (considered as) a letter, rather Alif is a letter, Laam is a letter, and Meem is a letter.” [At-Tirmidhi, Ad-Darimi] “There is no envy (acceptable) except in two (cases): a person whom Allah has given the Qur’an and recites it throughout the night and throughout the day. And a person whom Allah has given wealth, that he gives out throughout the night and throghout the day.” [Al-Bukhari and Muslim] It was narrated that Abdullah ibn Mas’ud said: Whoever reads Tabarakallahi Biyadihil Mulk [i.e. Surah al-Mulk] every night, Allah will protect him from the torment of the grave. At the time of the Messenger of Allah (Peace be upon him) we used to call it al-mani’ah (that which protects). In the Book of Allah it is a surah which, whoever recites it every night has done very well. (an-Nasa’i) Abdullah Ibn ‘Abbas and Anas Ibn Malik (Ra) reported that the Prophet (Peace be upon him) said, ‘Whoever recited Surah Zilzilah (99) would get the reward of reciting half the Qur’an. Whoever recited Surah al Kaafirun (109) would get a reward as if reading a quarter of the Qur’an. Whoever recited Surah al Ikhlas (112) would get a reward as if reading one third of the Qur’an’. (At-Tirmidhi 2818/A) Reading, understanding and implementing the Qur’an in our daily lives The virtues of reciting the Qur’an are too numerous to list. In order to get closer to Allah we need to recite the Qur’an, understand it and implement it in our daily lives. We should make a target of reading at least a chapter a day. If one can’t manage that then at least half a chapter. If one still can’t manage that then recite at least quarter of a chapter or even a page a day. However much we can manage we should try to recite each day with its meanings and implement what we learn into our daily lives. Reading a little each day is better than reciting a lot once in a while. We should build a close relationship with the Qur’an which is in fact building a close relationship with Allah! “Verily Allah raises nations by this book (the Qur’an) and puts down (i.e. destroys) others by it.” [Muslim]",
                     group1) { CreatedOn = "Group", CreatedTxt = "Step Points", CreatedOnTwo = "Item", CreatedTxtTwo = "Building a close relationship with the Qur’an", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Draw Closer to ALLAH" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "Keeping good company",
                     "One of the most important things we must do, which sadly many people neglect, is that we should avoid bad company. People we should avoid taking as friends those who speak too freely, who miss Salah, who do not dress modestly, who backbite, slander etc. The company of such people is poison",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nOne of the most important things we must do, which sadly many people neglect, is that we should avoid bad company. People we should avoid taking as friends those who speak too freely, who miss Salah, who do not dress modestly, who backbite, slander etc. The company of such people is poison; just even sitting and talking with them will lead one to commit sins. Just as a person who sits for a long time with a perfume seller begins to smell nice, and a person who sits by a gutter cleaner begins to smell awful, similarly a person who spends time in the company of the wicked eventually gets affected badly by them. Rather, we should seek out pious friends who fear Allah taala and who have the qualities of humility, charity, compassion, modesty and knowledge. If we sit with them we will always benefit and they will be a means for us to get closer to Allah taala! The Prophet (saws) said, “The case of the good companion and the bad companion is like that of the seller of musk and the blower of the bellows (iron-smith). As for the seller of musk, he will either give you some of the musk, or you will purchase some from him, or at least you will come away having experienced its good smell. Whereas the blower of the bellows will either burn your clothing, or at least you will come away having experienced its repugnant smell.” [Al-Bukhaaree and Muslim]\n\nRemember: “All friends will be enemies of one another on that Day (Day of Judgment) except those of the virtuous.” (al-Qur’an 43:67)",
                     group1) { CreatedOn = "Group", CreatedTxt = "Step Points", CreatedOnTwo = "Item", CreatedTxtTwo = "Keeping good company", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Draw Closer to ALLAH" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Having fear and hope in Allah",
                     "Indeed, no one despairs of relief from Allah except the disbelieving people. (Surah Yusuf 12:87) One must be hopeful of Allahs mercy and forgiveness and fearful of His punishment. It is this fear that should lead one to seek Allahs forgiveness with hope. Allah says: Know that Allah is severe in punishment and that Allah is Forgiving and Merciful.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nIndeed, no one despairs of relief from Allah except the disbelieving people. (Surah Yusuf 12:87) One must be hopeful of Allahs mercy and forgiveness and fearful of His punishment. It is this fear that should lead one to seek Allahs forgiveness with hope. Allah says: Know that Allah is severe in punishment and that Allah is Forgiving and Merciful. (Surat al-Maaidah 5:98) In the above verse, Allah gives us reason to fear because His punishment is justly severe, as well as good reason to have hope, since He is the Most Forgiving and Merciful. There should be a balance between fear and hope and not too much or too less of one or the other. We should always remain between fear and hope. For, the right and the approved kind of fear is that which acts as a barrier between the slave and the things forbidden by Allah. But, if fear is excessive, then the possibility is that the man will fall into despair and pessimism. On the other hand the approved state of optimism is of a man who does good in the light of the Shari'ah and is hopeful of being rewarded for it. Or, conversely, if a man committed a sin, he repents sincerely, and is hopeful of being forgiven. Allah (swt) said: Verily, those who believed, and those who migrated and fought in the way of Allah, it is they who are hopeful of Allah's mercy. And Allah is very Forgiving, very Merciful. (Al-Baqarah, 218) In contrast, if a man indulges in sins and excesses, but is hopeful that he would be forgiven without doing anything good, then, this is self-deception, mere illusion and false hope. Abu 'All Rowzbari has said, Fear and hope are like the two wings of a bird. If they are well balanced, the flight will be well balanced. But, If one is stunted, the Right would also be stunted. And, to be sure, if the two are lost, the bird will soon be in the throes of death. Allah has praised the people of hope and fear in the following verse:\n\nIs one who worships devotedly during the night, prostrating himself or standing, fearing the Hereafter, and hoping for the mercy of his Lord (is equal to him who doesn't do these things)?' (Al Zumar, 9) Hope then also demands fear. If that was not the case, one would be in a state of false security. Conversely, fear demands hope. Without that it would be despair. Fear and hope, both should be equally proportioned in our hearts, in our worship, and in our dua to Allah. Allah says: Call out to Him with fear and hope. (Surat al-Araaf 7:56) They forsake their beds to call their Lord in fear and hope. (Surat as-Sajdah 32:16)",
                     group1) { CreatedOn = "Group", CreatedTxt = "Step Points", CreatedOnTwo = "Item", CreatedTxtTwo = "Having fear and hope in Allah", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Draw Closer to ALLAH" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "Voluntary fasting for the pleasure of Allah",
                     "Fasting in general and voluntary fasting in particular is a great worship. Fasting is not restricted to Ramadhan, but it is an act of worship that can be [and should be in some cases] performed at any time and at any place except when not recommended.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nFasting in general and voluntary fasting in particular is a great worship. Fasting is not restricted to Ramadhan, but it is an act of worship that can be [and should be in some cases] performed at any time and at any place except when not recommended. Indeed, it is a worship that draws the believer closer to Allah and closer to perfection. Fasting Mondays and Thursdays: `Aa'ishah said: The Messenger of Allah, salla Allahu alaihi wa salam, used to fast Mondays and Thursdays. [An-Nasaa’i; Sahih] Abu Hurairah reported that the most the Prophet, salla Allahu alaihi wa salam, would fast would be Monday and Thursday. He was asked about that and he said: The deeds of people are presented to Allah on every Monday and Thursday. Allah forgives every Muslim except for those who are deserting each other. He says: leave them for later. [Ahmad; Hasan] Intention for voluntary fasting\n\nAs opposed to Ramadan, the intention does not have to be made before dawn. The person can intend fasting [and start fasting] after dawn any time [even after noon] given that he did not eat anything. `Aa'ishah said : The Prophet, salla Allahu alaihi wa sallam, came to us one day and said, Do you have any [food]? We said No. He said: Therefore, I am Fasting. [Muslim and Abu Dawood]  Fasting three days of every month(White days):\n\nAbu Tharr Al-Ghefari said: The Messenger of Allah, salla Allahu alaihi wa sallam, said O Abu Tharr! if you fast three days of every month, then fast the 13th, the 14th and the 15th [these are call the al-ayaam al-beedh, the white days]. [Ahmad, an-Nasaa'i and at-Tirmithi; Sahih] “Fasting and the Qur’an will intercede for the slave on the Day of Resurrection. Fasting will say: ‘O My Rabb! I prevented him from food and desires, so accept my intercession for him.’ And the Qur’an will say: ‘I prevented him from sleep during the night, so accept my intercession for him.’ He (sallallahu `alayhi wa sallam) said: ‘And they will (be allowed to) intercede.’” [Ahmad, at-Tabarani, Al-Hakim, Sahih] \n\nSo let us get closer to Allah by fasting Mondays and Thursdays or at least 3 days every month on the 14th,15th and 16th. If we leave something for the pleasure of Allah then we will get MUCH greater in return!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Step Points", CreatedOnTwo = "Item", CreatedTxtTwo = "Voluntary fasting for the pleasure of Allah", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Draw Closer to ALLAH" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Sincerely repenting to Allah",
                     "Allah says: O son of Adam, if your sins were to reach the clouds of the sky and you would then seek My forgiveness, I would forgive you. When a person sins and then sincerely turns to Allah for forgiveness, one will find Allah ready to accept his repentance and to forgive him, as this verse indicates.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAllah says: O son of Adam, if your sins were to reach the clouds of the sky and you would then seek My forgiveness, I would forgive you. When a person sins and then sincerely turns to Allah for forgiveness, one will find Allah ready to accept his repentance and to forgive him, as this verse indicates: And whoever does a wrong or wrongs himself, but then seeks forgiveness from Allah, he will find Allah forgiving and merciful. (Surat an-Nisaa 4:110) Everyone commits sin and does wrong, but Allah is always willing to forgive and He always gives them a chance to repent and seek His forgiveness. A believer should never forget the fact that Allah is so forgiving. If Allah had willed, He could have held everyone accountable for his or her sins, but He has decreed that He shall allow His servants to seek His forgiveness and that He shall in fact forgive who and what He wills. In fact, Allah commands that His servants seek His forgiveness:\n\nAnd seek Allahs forgiveness. Certainly, Allah is Forgiving, Merciful.(Surat al-Muzzammil 73:20) Repentance is an act, which purifies the soul and brings the servant closer to Allah. It puts the heart at rest from guilt. It protects one from falling prey to his desires and lusts and increases his faith.\n\nWe must ask ourselves this question: Would we be willing to forgive anyone who hurts us and disobeys us constantly as easily as Allah is Able to forgive? Most probably, the answer would be no. But our Creator is the Most Kind and He is the Most Perfect. Lo! Allah is a Lord of Kindness to mankind, but most of mankind give not thanks. (Surat al-Baqarah 2:143) In this Hadithi Qudsi, mankind is encouraged to seek Allahs forgiveness and repent, but there are five conditions of repentance, which must be met for ones repentance to be accepted. The first and most important is that the act of repentance be sincerely for Allah alone. Secondly, the person must feel remorse and guilt over his actions so much so that he wished he had never done it in the first place. The third condition is that the person must immediately cease performing the wrong and sinful act. Fourthly, the repentant person must have a firm intention to never commit the sin again. And lastly, the person must repent before it is too late, meaning before death approaches.\n\nHowever, there is a condition. One must not associate any partners with Allah, which is shirk. And Allah does not forgive shirk and if one dies without believing in Allah alone as ones Creator, then he will be doomed to the Hellfire for all of eternity. So, Allah emphasizes the importance of calling on Him alone. He has no and needs no partners, associates, wives, children, etc. There is no god, but Allah. None forgives sins except Him, so one who is seeking forgiveness should seek it only from Allah. Allah's forgiveness and mercy is far greater and vaster than the sins of the creation. One must always have trust and hope in Allah in both good times and bad times and especially when seeking Allahs forgiveness. And the believer who calls out to his Lord for forgiveness demonstrates his true weakness and that he is totally dependent on the Creator. \n\nWhen one confesses his sins to Allah and sincerely repents with hope in Allahs mercy, the heart should come to peace and the soul should feel rest. When a person has hope, he has no reason to despair because it only leads to destruction. Allah gives hope to all, especially those who despair that there is no reason to despair because Allah is the Most Merciful of all those who show mercy. Allah praises those who repent and turn to Him: \n\nAnd those who, when they commit a lewd act or wrong themselves with evil, remember Allah and ask forgiveness for their sins and who forgives sins except Allah? And they do not persist in what (wrong) they were doing while they knew it. For such, the reward is forgiveness from their Lord and Gardens with rivers flowing through, wherein they shall abide forever. How excellent is the reward of the doers (of good)! (Surah Ali Imran 3:135-136)",
                     group1) { CreatedOn = "Group", CreatedTxt = "Step Points", CreatedOnTwo = "Item", CreatedTxtTwo = "Sincerely repenting to Allah", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Draw Closer to ALLAH" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "Having good manners character and being humble",
                     "Many of us think that a perfect Muslim is simply one who is correct in the observance of the salah (ritual Prayer), the fasting, the zakah (payment of a certain portion of one’s wealth to the poor), and the Hajj (pilgrimage to Makkah). This indeed is not the case.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nMany of us think that a perfect Muslim is simply one who is correct in the observance of the salah (ritual Prayer), the fasting, the zakah (payment of a certain portion of one’s wealth to the poor), and the Hajj (pilgrimage to Makkah). This indeed is not the case. If the ritual observances do not help the person to be humble, virtuous and truly God-fearing, then he or she is not a real Muslim. A Muslim should be good and just in dealing with others, no matter their religion, and take special care to keep away from all the shameful and sinful things Allah (God) has forbidden. One can never get close to Allah by being arrogant, full of pride and having a bad character and manners. Those who have humility and are humble and have good character and manners are the closest to Allah and Allah raises their ranks in the hereafter. The superiority of good character:\n\nHadrat Abu Darda, may Allah be pleased with him, relates that the Holy Prophet Muhammad, may the peace and blessings of Allah be upon him, said, “Nothing is heavier in the scales of a believer on the Day of Judgement than his good behaviour. Allah detests a person who is obscene and shameless”. (Tirmidhi) Having humility and being humble: The Prophet (PBUH) said: He who was humble for the sake of God by one degree, God (SWT) would then elevate them to a degree till they reach the uppermost of high Orders, and he who was arrogant to God (SWT), God (SWT) would then lower him for a degree till he reaches the lowest of low Orders, (Narrated by: Muslim (Hadeeth: 6535) Al-Nawawi said:  The Prophet (peace and blessings of Allaah be upon him) said: “And no one humbles himself before Allaah but Allaah will raise him (in status).” Humbleness is to know the value of oneself, to avoid pride, or disregarding the truth and underestimating people. As the Prophet sallallahu `alaihi wa sallam said, according to Muslim and others, Al-Kibr is rejecting the truth and looking down upon people [Muslim, Tirmidhi and Abu Dawud]. Humbleness is for one who is important and significant and he fears to gain notoriety or to become too great among people. Humbleness is that one should humble himself with his companions. Humbleness is to humble oneself to one who is below you. If you find someone who is younger than you, or of less importance than you, you should not despise him, because he might have a better heart than you, or be less sinful, or closer to Allah than you. Even if you see a sinful person and you are righteous, do not act in arrogance towards him, and thank Allah that He saved you from the tribulation that He put him through. Remember that there might be some riyaa' or vanity in your righteous deeds that may cause them to be of no avail, and that this sinful person may be regretful and fearful concerning his bad deeds, and this may be the cause of forgiveness of his sins. Humbleness is that your deed should not become too great in your eyes. If you do a good deed, or attempt to get closer to Allah ta`ala through an act of obedience, your deed may still not be accepted, Allah only accepts from those who have taqwa (fear of Allah). (Surat al-Maida: 27)",
                     group1) { CreatedOn = "Group", CreatedTxt = "Step Points", CreatedOnTwo = "Item", CreatedTxtTwo = "Having good manners character and being humble", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Draw Closer to ALLAH" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}
