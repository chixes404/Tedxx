﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tedx.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class User {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal User() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Tedx.Resources.User", typeof(User).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Age is required.
        /// </summary>
        public static string AgeRequired {
            get {
                return ResourceManager.GetString("AgeRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid age format or range..
        /// </summary>
        public static string AgeValidation {
            get {
                return ResourceManager.GetString("AgeValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email is already exists..
        /// </summary>
        public static string EmailAlreadyExists {
            get {
                return ResourceManager.GetString("EmailAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email is required.
        /// </summary>
        public static string EmailRequired {
            get {
                return ResourceManager.GetString("EmailRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email is invalid.
        /// </summary>
        public static string EmailVaildation {
            get {
                return ResourceManager.GetString("EmailVaildation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Full name must be between {1} and {2} characters long.
        /// </summary>
        public static string FullNameLength {
            get {
                return ResourceManager.GetString("FullNameLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Full name must contain only letters and spaces.
        /// </summary>
        public static string FullNameLettersOnly {
            get {
                return ResourceManager.GetString("FullNameLettersOnly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name is required.
        /// </summary>
        public static string FullNameRequired {
            get {
                return ResourceManager.GetString("FullNameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Job is required.
        /// </summary>
        public static string JobRequired {
            get {
                return ResourceManager.GetString("JobRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field must contain at most 300 words..
        /// </summary>
        public static string MaxWordsValidation {
            get {
                return ResourceManager.GetString("MaxWordsValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Phone is already exists..
        /// </summary>
        public static string PhoneAlreadyExists {
            get {
                return ResourceManager.GetString("PhoneAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Phone is invalid.
        /// </summary>
        public static string PhoneInvalid {
            get {
                return ResourceManager.GetString("PhoneInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Phone is Required.
        /// </summary>
        public static string PhoneRequired {
            get {
                return ResourceManager.GetString("PhoneRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Role is required.
        /// </summary>
        public static string RoleIsRequired {
            get {
                return ResourceManager.GetString("RoleIsRequired", resourceCulture);
            }
        }
    }
}
