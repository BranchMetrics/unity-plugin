# MAT Unity Plugin

### Event Items

While an event is like your receipt for a purchase, the event items are the individual items you purchased.
Event items allow you to define multiple items for a single event. The “trackAction” method can include this event item data.

When using the function to track events with event items, this public struct has to be defined in the C# script:

	public struct MATEventItem {
		public string    item;
		public double    unitPrice;
		public int       quantity;
		public double    revenue;
	}

The function for tracking event items looks like this:

	[DllImport ("__Internal")]
	private static extern void trackActionWithEventItem(
		string eventName,
		bool isId,
		MATEventItem[] items,
		int eventItemCount,
		string refId,
		double revenue,
		string currency,
		int transactionState,
		string receiptData,
		string receiptSignature);

__Parameters:__

- eventName - the event name associated with the event
- isId - whether the event ID (true) or event name (false) is being passed in
- items - an array of event items
- eventItemCount - Length of array
- refId - the advertiser reference ID you would like to associate with this event
- revenue - the revenue amount associated with the event
- currency - the ISO 4217 currency code for the revenue
- transactionState - the purchase status received from app store
- receiptData - the receipt data from iTunes/Google Play
- receiptSignature - the receipt signature from Google Play, not used on iOS

Sample tracking code:

	// Create a couple event items we wish to track
	MATEventItem item1 = new MATEventItem();
	item1.item = "apple";
	item1.unitPrice = 5;
	item1.quantity = 5;
	item1.revenue = 3;

	MATEventItem item2 = new MATEventItem();
	item2.item = "banana";
	item2.unitPrice = 1;
	item2.quantity = 3;
	item2.revenue = 1.5;

	// Add them to an array of event items to pass in to tracking function
	MATEventItem[] arr = { item1, item2 };

	// If we have an id we want to associate with this event, pass in refId field
	string refId = "some_order_id";

	// Any extra revenue that might be generated over and above the revenues generated from event items.
	// This will be added on top of the event item revenues
	// Total event revenue = sum of event item revenues in arr + extraRevenue
	float extraRevenue = 0; // default to zero

	// Transaction state may be set to the value received from iOS/Android app store when a purchase transaction is finished. 
	int transactionState = 1;

	// Receipt data, receipt signature are from iOS/Android app store and used for purchase verification.
	string receiptData = null;
	string receiptSignature = null;

	// Track the event items with MAT
	trackActionWithEventItem("itemPurchase", false, arr, arr.Length, refId, extraRevenue, "USD", transactionState, receiptData, receiptSignature);
