﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Horse_Isle_Server.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Horse_Isle_Server.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;cross-domain-policy&gt;
        ///	&lt;allow-access-from domain=&quot;*&quot; to-ports=&quot;1080&quot; secure=&quot;false&quot;/&gt;
        ///&lt;/cross-domain-policy&gt;.
        /// </summary>
        internal static string DefaultCrossDomain {
            get {
                return ResourceManager.GetString("DefaultCrossDomain", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to # Horse Isle Server Default Configuration File
        ///
        ///# Ip address the server will bind to (default: 0.0.0.0 ALL INTERFACES)
        ///ip=0.0.0.0
        ///# Port the server will bind to (default: 1080)
        ///port=1080
        ///
        ///# MariaDB Database 
        ///db_ip=127.0.0.1
        ///db_username=root
        ///db_password=test123
        ///db_port=3306
        ///
        ///# Map Data
        ///map=MapData.bmp
        ///overlaymap=oMapData.bmp
        ///
        ///# Cross-Domain Policy File
        ///crossdomain=&quot;CrossDomainPolicy.xml
        ///
        ///# Should print debug logs
        ///debug=false.
        /// </summary>
        internal static string DefaultServerProperties {
            get {
                return ResourceManager.GetString("DefaultServerProperties", resourceCulture);
            }
        }
    }
}
