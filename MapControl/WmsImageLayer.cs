﻿// XAML Map Control - https://github.com/ClemensFischer/XAML-Map-Control
// © 2017 Clemens Fischer
// Licensed under the Microsoft Public License (Ms-PL)

using System;
#if NETFX_CORE
using Windows.UI.Xaml;
#else
using System.Windows;
#endif

namespace MapControl
{
    public partial class WmsImageLayer : MapImageLayer
    {
        public static readonly DependencyProperty ServerUriProperty = DependencyProperty.Register(
            "ServerUri", typeof(Uri), typeof(WmsImageLayer),
            new PropertyMetadata(null, (o, e) => ((WmsImageLayer)o).UpdateImage()));

        public static readonly DependencyProperty VersionProperty = DependencyProperty.Register(
            "Version", typeof(string), typeof(WmsImageLayer),
            new PropertyMetadata("1.3.0", (o, e) => ((WmsImageLayer)o).UpdateImage()));

        public static readonly DependencyProperty LayersProperty = DependencyProperty.Register(
            "Layers", typeof(string), typeof(WmsImageLayer),
            new PropertyMetadata(null, (o, e) => ((WmsImageLayer)o).UpdateImage()));

        public static readonly DependencyProperty StylesProperty = DependencyProperty.Register(
            "Styles", typeof(string), typeof(WmsImageLayer),
            new PropertyMetadata(null, (o, e) => ((WmsImageLayer)o).UpdateImage()));

        public static readonly DependencyProperty ParametersProperty = DependencyProperty.Register(
            "Parameters", typeof(string), typeof(WmsImageLayer),
            new PropertyMetadata(null, (o, e) => ((WmsImageLayer)o).UpdateImage()));

        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register(
            "Format", typeof(string), typeof(WmsImageLayer),
            new PropertyMetadata("image/png", (o, e) => ((WmsImageLayer)o).UpdateImage()));

        public static readonly DependencyProperty TransparentProperty = DependencyProperty.Register(
            "Transparent", typeof(bool), typeof(WmsImageLayer),
            new PropertyMetadata(false, (o, e) => ((WmsImageLayer)o).UpdateImage()));

        private string layers = string.Empty;

        public Uri ServerUri
        {
            get { return (Uri)GetValue(ServerUriProperty); }
            set { SetValue(ServerUriProperty, value); }
        }

        public string Version
        {
            get { return (string)GetValue(VersionProperty); }
            set { SetValue(VersionProperty, value); }
        }

        public string Layers
        {
            get { return (string)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        public string Styles
        {
            get { return (string)GetValue(StylesProperty); }
            set { SetValue(StylesProperty, value); }
        }

        public string Parameters
        {
            get { return (string)GetValue(ParametersProperty); }
            set { SetValue(ParametersProperty, value); }
        }

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public bool Transparent
        {
            get { return (bool)GetValue(TransparentProperty); }
            set { SetValue(TransparentProperty, value); }
        }

        protected override bool UpdateImage(BoundingBox boundingBox)
        {
            if (ServerUri == null)
            {
                return false;
            }

            var version = Version ?? "1.3.0";
            var queryParameters = ParentMap.MapProjection.WmsQueryParameters(boundingBox, version);

            if (string.IsNullOrEmpty(queryParameters))
            {
                return false;
            }

            var query = "?SERVICE=WMS"
                + "&VERSION=" + version
                + "&REQUEST=GetMap"
                + "&LAYERS=" + (Layers ?? string.Empty)
                + "&STYLES=" + (Styles ?? string.Empty)
                + "&" + queryParameters
                + "&FORMAT=" + (Format ?? "image/png")
                + "&TRANSPARENT=" + (Transparent ? "TRUE" : "FALSE");

            if (!string.IsNullOrEmpty(Parameters))
            {
                query += "&" + Parameters;
            }

            var uri = new Uri(ServerUri, query.Replace(" ", "%20"));

            UpdateImage(uri);
            return true;
        }
    }
}
