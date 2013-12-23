using System;
using Android.Widget;

namespace Esri.Android.Map.Popup
{
	public partial class ArcGISAttachmentsAdapter
	{
		Java.Lang.Object IAdapter.GetItem (int position)
		{
			return GetItem (position);
		}
	}

	public partial class ArcGISMediaAdapter
	{
		Java.Lang.Object IAdapter.GetItem (int position)
		{
			return GetItem (position);
		}
	}

	public partial class ArcGISLayout : global::Esri.Android.Map.Popup.IPopupLayout
	{
		global::Android.Views.View IPopupLayout.AttachmentsView {
			get { return AttachmentsView; }
			set { SetAttachmentsView (value); }
		}

		global::Android.Views.View IPopupLayout.AttributesView {
			get { return AttributesView; }
			set { SetAttributesView (value); }
		}

		global::Android.Views.ViewGroup IPopupLayout.Layout {
			get { return Layout; }
		}

		global::Android.Views.View IPopupLayout.MediaView {
			get { return MediaView; }
			set { SetMediaView (value); }
		}

		global::Esri.Android.Map.Popup.IPopupStyle IPopupLayout.Style {
			get { return Style; }
		}

		global::Android.Views.View IPopupLayout.TitleView {
			get { return TitleView; }
			set { SetTitleView (value); }
		}
	}

	public partial class ArcGISEditAttributesAdapter
	{
		public override Java.Lang.Object GetItem (int p0)
		{
			return GetItem (p0);
		}
	}
}

namespace Esri.Core.Renderer
{
	public partial class MultipartColorRamp : global::Java.Util.IList
	{
		bool Java.Util.IList.Add (Java.Lang.Object @object)
		{
			return Add ((ColorRamp)@object);
		}

		void Java.Util.IList.Add (int location, Java.Lang.Object @object)
		{
			Add (location, (ColorRamp)@object);
		}

		static System.Collections.Generic.List<TT> GetCollection<TT> (System.Collections.ICollection collection)
		{
			var collection2 = new System.Collections.Generic.List<TT> ();
			collection2.ToArray ();
			foreach (TT item in collection)
				collection2.Add (item);
			return collection2;
		}

		bool Java.Util.IList.AddAll (int location, System.Collections.ICollection collection)
		{
			var collection2 = GetCollection<ColorRamp> (collection);

			return AddAll (location, collection2);
		}

		bool Java.Util.IList.AddAll (System.Collections.ICollection collection)
		{
			var collection2 = GetCollection<ColorRamp> (collection);

			return AddAll (collection2);
		}

		bool Java.Util.IList.ContainsAll (System.Collections.ICollection collection)
		{
			var collection2 = GetCollection<object> (collection);

			return ContainsAll (collection2);
		}

		bool Java.Util.IList.RemoveAll (System.Collections.ICollection collection)
		{
			var collection2 = GetCollection<object> (collection);

			return RemoveAll (collection2);
		}

		bool Java.Util.IList.RetainAll (System.Collections.ICollection collection)
		{
			var collection2 = GetCollection<object> (collection);

			return RetainAll (collection2);
		}

		Java.Lang.Object Java.Util.IList.Set (int location, Java.Lang.Object @object)
		{
			return Set (location, (ColorRamp)@object);
		}

		bool Java.Util.ICollection.Add (Java.Lang.Object @object)
		{
			return Add ((ColorRamp)@object);
		}

		bool Java.Util.ICollection.AddAll (System.Collections.ICollection collection)
		{
			var collection2 = GetCollection<ColorRamp> (collection);

			return AddAll (collection2);
		}

		bool Java.Util.ICollection.ContainsAll (System.Collections.ICollection collection)
		{
			var collection2 = GetCollection<object> (collection);

			return ContainsAll (collection2);
		}

		bool Java.Util.ICollection.RemoveAll (System.Collections.ICollection collection)
		{
			var collection2 = GetCollection<object> (collection);

			return RemoveAll (collection2);
		}

		bool Java.Util.ICollection.RetainAll (System.Collections.ICollection collection)
		{
			var collection2 = GetCollection<object> (collection);

			return RetainAll (collection2);
		}

		Java.Lang.Object Java.Util.IList.Remove (int p0)
		{
			return Remove (p0);
		}

		Java.Lang.Object Java.Util.IList.Get (int p0)
		{
			return Get (p0);
		}

		System.Collections.IList Java.Util.IList.SubList (int p0, int p1)
		{
			var list =  SubList (p0, p1);
			var ret = new System.Collections.ArrayList ();
			foreach (object obj in list)
				ret.Add (obj);

			return ret;
		}
	}
}

namespace Esri.Core.Internal.Widget {

	public partial class InfiniteGallery
	{
		protected override Java.Lang.Object RawAdapter 
		{
			get { return (Java.Lang.Object)Adapter; }
			set { Adapter = (global::Android.Widget.IAdapter)value; }
		}
	}

}

namespace Esri.Core.Symbol {

	public partial class MultiLayerSymbol : global::Esri.Core.Symbol.ISymbol
	{
		global::Esri.Core.Symbol.ISymbol ISymbol.Copy ()
		{
			return Copy ();
		}
	}
}


