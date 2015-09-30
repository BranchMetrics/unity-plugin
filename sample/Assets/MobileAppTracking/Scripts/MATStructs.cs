using System;

namespace MATSDK
{
    /// <para>
    /// Enum used for MAT ad request gender.
    /// </para>
    public enum MATAdGender
    {
        UNKNOWN,
        MALE,
        FEMALE
    }

    /// <para>
    /// Enum used for MAT banner ad position.
    /// </para>
    public enum MATBannerPosition
    {
        BOTTOM_CENTER,
        TOP_CENTER
    }

    /// <para>
    /// Struct used for MAT events.
    /// </para>
    public struct MATEvent
    {
        public string       name;
        public int?         id;
        public double?      revenue;
        public string       currencyCode;
        public string       advertiserRefId;
        public MATItem[]   eventItems;
        public int?         transactionState;
        public string       receipt;
        public string       receiptSignature;
        public string       contentType;
        public string       contentId;
        public int?         level;
        public int?         quantity;
        public string       searchString;
        public double?      rating;
        public DateTime?    date1;
        public DateTime?    date2;
        public string       attribute1;
        public string       attribute2;
        public string       attribute3;
        public string       attribute4;
        public string       attribute5;

        private MATEvent(int dummy1, int dummy2) {
            this.name               = null;
            this.id                 = null;
            this.revenue            = null;
            this.currencyCode       = null;
            this.advertiserRefId    = null;
            this.eventItems         = null;
            this.transactionState   = null;
            this.receipt            = null;
            this.receiptSignature   = null;
            this.contentType        = null;
            this.contentId          = null;
            this.level              = null;
            this.quantity           = null;
            this.searchString       = null;
            this.rating             = null;
            this.date1              = null;
            this.date2              = null;
            this.attribute1         = null;
            this.attribute2         = null;
            this.attribute3         = null;
            this.attribute4         = null;
            this.attribute5         = null;
        }

        public MATEvent(string name) : this(0, 0) {
            this.name               = name;
        }

        public MATEvent(int id) : this(0, 0) {
            this.id                 = id;
        }
    }

    internal struct MATEventIos
    {
        public string        name;
        public string        eventId;
        public string        revenue;
        public string        currencyCode;
        public string        advertiserRefId;
        public string        transactionState;
        public string        contentType;
        public string        contentId;
        public string        level;
        public string        quantity;
        public string        searchString;
        public string        rating;
        public string        date1;
        public string        date2;
        public string        attribute1;
        public string        attribute2;
        public string        attribute3;
        public string        attribute4;
        public string        attribute5;
        
        private MATEventIos(int dummy1, int dummy2) {
            this.eventId            = null;
            this.name               = null;
            this.revenue            = null;
            this.currencyCode       = null;
            this.advertiserRefId    = null;
            this.transactionState   = null;
            this.contentType        = null;
            this.contentId          = null;
            this.level              = null;
            this.quantity           = null;
            this.searchString       = null;
            this.rating             = null;
            this.date1              = null;
            this.date2              = null;
            this.attribute1         = null;
            this.attribute2         = null;
            this.attribute3         = null;
            this.attribute4         = null;
            this.attribute5         = null;
        }

        public MATEventIos(string name) : this(0, 0) {
            this.name = name;
        }
        
        public MATEventIos(int id) : this(0, 0) {
            this.eventId = id.ToString();
        }

        public MATEventIos(MATEvent matEvent)
        {
            name                = matEvent.name;
            eventId             = null == matEvent.name ? matEvent.id.ToString() : null;
            advertiserRefId     = matEvent.advertiserRefId;
            attribute1          = matEvent.attribute1;
            attribute2          = matEvent.attribute2;
            attribute3          = matEvent.attribute3;
            attribute4          = matEvent.attribute4;
            attribute5          = matEvent.attribute5;
            contentId           = null == matEvent.contentId ? null : matEvent.contentId.ToString();
            contentType         = matEvent.contentType;
            currencyCode        = matEvent.currencyCode;
            level               = null == matEvent.level ? null : matEvent.level.ToString();
            quantity            = null == matEvent.quantity ? null : matEvent.quantity.ToString();
            rating              = null == matEvent.rating ? null : matEvent.rating.ToString();
            revenue             = null == matEvent.revenue ? null : matEvent.revenue.ToString();
            searchString        = matEvent.searchString;
            transactionState    = null == matEvent.transactionState ? null : matEvent.transactionState.ToString();
            date1               = null;
            date2               = null;

            // datetime starts in 1970
            DateTime datetime = new DateTime(1970, 1, 1);
            
            if(matEvent.date1.HasValue)
            {
                double millis = new TimeSpan(matEvent.date1.Value.Ticks).TotalMilliseconds;
                double millisFrom1970 = millis - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
                date1 = millisFrom1970.ToString();
            }
            
            if(matEvent.date2.HasValue)
            {
                double millis = new TimeSpan(matEvent.date2.Value.Ticks).TotalMilliseconds;
                double millisFrom1970 = millis - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
                date2 = millisFrom1970.ToString();
            }
        }
    }
    
    /// <para>
    /// Struct used for MAT event items.
    /// </para>
    public struct MATItem
    {
        public string   name;
        public double?  unitPrice;
        public int?     quantity;
        public double?  revenue;
        public string   attribute1;
        public string   attribute2;
        public string   attribute3;
        public string   attribute4;
        public string   attribute5;
        
        public MATItem(string name)
        {
            this.name       = name;
            this.unitPrice  = null;
            this.quantity   = null;
            this.revenue    = null;
            this.attribute1 = null;
            this.attribute2 = null;
            this.attribute3 = null;
            this.attribute4 = null;
            this.attribute5 = null;
        }
    }

    internal struct MATItemIos
    {
        public string   name;
        public double   unitPrice;
        public int      quantity;
        public double   revenue;
        public string   attribute1;
        public string   attribute2;
        public string   attribute3;
        public string   attribute4;
        public string   attribute5;

        public MATItemIos(string name)
        {
            this.name       = name;
            this.unitPrice  = 0;
            this.quantity   = 0;
            this.revenue    = 0;
            this.attribute1 = null;
            this.attribute2 = null;
            this.attribute3 = null;
            this.attribute4 = null;
            this.attribute5 = null;
        }

        public MATItemIos(MATItem matItem)
        {
            this.name       = matItem.name;
            this.unitPrice  = matItem.unitPrice ?? 0;
            this.quantity   = matItem.quantity ?? 0;
            this.revenue    = matItem.revenue ?? 0;
            this.attribute1 = matItem.attribute1;
            this.attribute2 = matItem.attribute2;
            this.attribute3 = matItem.attribute3;
            this.attribute4 = matItem.attribute4;
            this.attribute5 = matItem.attribute5;
        }
    }

    /// <para>
    /// Struct used for preloaded app attribution.
    /// </para>
    public struct MATPreloadData
    {
        public string advertiserSubAd;
        public string advertiserSubAdgroup;
        public string advertiserSubCampaign;
        public string advertiserSubKeyword;
        public string advertiserSubPublisher;
        public string advertiserSubSite;
        public string agencyId;
        public string offerId;
        public string publisherId;
        public string publisherReferenceId;
        public string publisherSub1;
        public string publisherSub2;
        public string publisherSub3;
        public string publisherSub4;
        public string publisherSub5;
        public string publisherSubAd;
        public string publisherSubAdgroup;
        public string publisherSubCampaign;
        public string publisherSubKeyword;
        public string publisherSubPublisher;
        public string publisherSubSite;

        public MATPreloadData(string publisherId)
        {
            this.advertiserSubAd = null;
            this.advertiserSubAdgroup = null;
            this.advertiserSubCampaign = null;
            this.advertiserSubKeyword = null;
            this.advertiserSubPublisher = null;
            this.advertiserSubSite = null;
            this.agencyId = null;
            this.offerId  = null;
            this.publisherId = publisherId;
            this.publisherReferenceId = null;
            this.publisherSub1 = null;
            this.publisherSub2 = null;
            this.publisherSub3 = null;
            this.publisherSub4 = null;
            this.publisherSub5 = null;
            this.publisherSubAd = null;
            this.publisherSubAdgroup = null;
            this.publisherSubCampaign = null;
            this.publisherSubKeyword = null;
            this.publisherSubPublisher = null;
            this.publisherSubSite = null;
        }
    }
}
