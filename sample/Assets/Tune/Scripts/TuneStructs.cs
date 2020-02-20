using System;

namespace TuneSDK
{
    /// <para>
    /// Struct used for TUNE events.
    /// </para>
    public struct TuneEvent
    {
        public string       name;
        public double?      revenue;
        public string       currencyCode;
        public string       advertiserRefId;
        public TuneItem[]   eventItems;
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
        public string       deviceForm;

        private TuneEvent(int dummy1, int dummy2) {
            this.name               = null;
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
            this.deviceForm         = null;
        }

        public TuneEvent(string name) : this(0, 0) {
            this.name               = name;
        }
    }

    internal struct TuneEventIos
    {
        public string        name;
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

        private TuneEventIos(int dummy1, int dummy2) {
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

        public TuneEventIos(string name) : this(0, 0) {
            this.name = name;
        }

        public TuneEventIos(TuneEvent tuneEvent)
        {
            name                = tuneEvent.name;
            advertiserRefId     = tuneEvent.advertiserRefId;
            attribute1          = tuneEvent.attribute1;
            attribute2          = tuneEvent.attribute2;
            attribute3          = tuneEvent.attribute3;
            attribute4          = tuneEvent.attribute4;
            attribute5          = tuneEvent.attribute5;
            contentId           = null == tuneEvent.contentId ? null : tuneEvent.contentId.ToString();
            contentType         = tuneEvent.contentType;
            currencyCode        = tuneEvent.currencyCode;
            level               = null == tuneEvent.level ? null : tuneEvent.level.ToString();
            quantity            = null == tuneEvent.quantity ? null : tuneEvent.quantity.ToString();
            rating              = null == tuneEvent.rating ? null : tuneEvent.rating.ToString();
            revenue             = null == tuneEvent.revenue ? null : tuneEvent.revenue.ToString();
            searchString        = tuneEvent.searchString;
            transactionState    = null == tuneEvent.transactionState ? null : tuneEvent.transactionState.ToString();
            date1               = null;
            date2               = null;

            // datetime starts in 1970
            DateTime datetime = new DateTime(1970, 1, 1);

            if(tuneEvent.date1.HasValue)
            {
                double millis = new TimeSpan(tuneEvent.date1.Value.Ticks).TotalMilliseconds;
                double millisFrom1970 = millis - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
                date1 = millisFrom1970.ToString();
            }

            if(tuneEvent.date2.HasValue)
            {
                double millis = new TimeSpan(tuneEvent.date2.Value.Ticks).TotalMilliseconds;
                double millisFrom1970 = millis - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
                date2 = millisFrom1970.ToString();
            }
        }
    }

    /// <para>
    /// Struct used for TUNE event items.
    /// </para>
    public struct TuneItem
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

        public TuneItem(string name)
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

    internal struct TuneItemIos
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

        public TuneItemIos(string name)
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

        public TuneItemIos(TuneItem tuneItem)
        {
            this.name       = tuneItem.name;
            this.unitPrice  = tuneItem.unitPrice ?? 0;
            this.quantity   = tuneItem.quantity ?? 0;
            this.revenue    = tuneItem.revenue ?? 0;
            this.attribute1 = tuneItem.attribute1;
            this.attribute2 = tuneItem.attribute2;
            this.attribute3 = tuneItem.attribute3;
            this.attribute4 = tuneItem.attribute4;
            this.attribute5 = tuneItem.attribute5;
        }
    }

    /// <para>
    /// Struct used for preloaded app attribution.
    /// </para>
    public struct TunePreloadData
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

        public TunePreloadData(string publisherId)
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
