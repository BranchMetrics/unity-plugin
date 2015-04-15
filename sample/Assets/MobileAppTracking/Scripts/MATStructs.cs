using System;

namespace MATSDK
{
    /// <para>
    /// Struct used for TUNE events.
    /// </para>
    public struct MATEvent
    {
        public string       name;
        public int?         id;
        public double?      revenue;
        public string       currencyCode;
        public string       advertiserRefId;
        public MATItem[]    eventItems;
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

        public MATEvent(string name) {
            this.name = name;
            this.id = null;
            this.revenue = null;
            this.currencyCode = null;
            this.advertiserRefId = null;
            this.eventItems = null;
            this.transactionState = null;
            this.receipt = null;
            this.receiptSignature = null;
            this.contentType = null;
            this.contentId = null;
            this.level = null;
            this.quantity = null;
            this.searchString = null;
            this.rating = 0;
            this.date1 = null;
            this.date2 = null;
            this.attribute1 = null;
            this.attribute2 = null;
            this.attribute3 = null;
            this.attribute4 = null;
            this.attribute5 = null;
        }

        public MATEvent(int id) {
            this.id = id;
            this.name = null;
            this.revenue = null;
            this.currencyCode = null;
            this.advertiserRefId = null;
            this.eventItems = null;
            this.transactionState = null;
            this.receipt = null;
            this.receiptSignature = null;
            this.contentType = null;
            this.contentId = null;
            this.level = null;
            this.quantity = null;
            this.searchString = null;
            this.rating = 0;
            this.date1 = null;
            this.date2 = null;
            this.attribute1 = null;
            this.attribute2 = null;
            this.attribute3 = null;
            this.attribute4 = null;
            this.attribute5 = null;
        }
    }

    /// <para>
    /// Struct used for storing MAT information.
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

    internal struct MATItemIOS
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
    }

    /// <para>
    /// Struct used for preloaded app attribution.
    /// </para>
    public struct MATPreloadData
    {
        public string publisherId;
        public string offerId;
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
        public string advertiserSubAd;
        public string advertiserSubAdgroup;
        public string advertiserSubCampaign;
        public string advertiserSubKeyword;
        public string advertiserSubPublisher;
        public string advertiserSubSite;

        public MATPreloadData(string publisherId)
        {
            this.publisherId = publisherId;
            this.offerId  = null;
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
            this.advertiserSubAd = null;
            this.advertiserSubAdgroup = null;
            this.advertiserSubCampaign = null;
            this.advertiserSubKeyword = null;
            this.advertiserSubPublisher = null;
            this.advertiserSubSite = null;
        }
    }
}