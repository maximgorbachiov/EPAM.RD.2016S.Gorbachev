﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientApplication.WcfService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserDataContract", Namespace="http://schemas.datacontract.org/2004/07/StorageInterfaces.CommunicationEntities.W" +
        "cfEntities")]
    [System.SerializableAttribute()]
    public partial class UserDataContract : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime DateOfBirthField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ClientApplication.WcfService.Gender GenderField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SecondNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ClientApplication.WcfService.CountryVisa[] VisasField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime DateOfBirth {
            get {
                return this.DateOfBirthField;
            }
            set {
                if ((this.DateOfBirthField.Equals(value) != true)) {
                    this.DateOfBirthField = value;
                    this.RaisePropertyChanged("DateOfBirth");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ClientApplication.WcfService.Gender Gender {
            get {
                return this.GenderField;
            }
            set {
                if ((this.GenderField.Equals(value) != true)) {
                    this.GenderField = value;
                    this.RaisePropertyChanged("Gender");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SecondName {
            get {
                return this.SecondNameField;
            }
            set {
                if ((object.ReferenceEquals(this.SecondNameField, value) != true)) {
                    this.SecondNameField = value;
                    this.RaisePropertyChanged("SecondName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ClientApplication.WcfService.CountryVisa[] Visas {
            get {
                return this.VisasField;
            }
            set {
                if ((object.ReferenceEquals(this.VisasField, value) != true)) {
                    this.VisasField = value;
                    this.RaisePropertyChanged("Visas");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Gender", Namespace="http://schemas.datacontract.org/2004/07/StorageInterfaces.Entities")]
    public enum Gender : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Man = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Woman = 1,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CountryVisa", Namespace="http://schemas.datacontract.org/2004/07/StorageInterfaces.Entities")]
    [System.SerializableAttribute()]
    public partial struct CountryVisa : System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string countryField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime endField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime startField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string country {
            get {
                return this.countryField;
            }
            set {
                if ((object.ReferenceEquals(this.countryField, value) != true)) {
                    this.countryField = value;
                    this.RaisePropertyChanged("country");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime end {
            get {
                return this.endField;
            }
            set {
                if ((this.endField.Equals(value) != true)) {
                    this.endField = value;
                    this.RaisePropertyChanged("end");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime start {
            get {
                return this.startField;
            }
            set {
                if ((this.startField.Equals(value) != true)) {
                    this.startField = value;
                    this.RaisePropertyChanged("start");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WcfService.IServiceContract")]
    public interface IServiceContract {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceContract/AddUser", ReplyAction="http://tempuri.org/IServiceContract/AddUserResponse")]
        int AddUser(ClientApplication.WcfService.UserDataContract userData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceContract/AddUser", ReplyAction="http://tempuri.org/IServiceContract/AddUserResponse")]
        System.Threading.Tasks.Task<int> AddUserAsync(ClientApplication.WcfService.UserDataContract userData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceContract/DeleteUser", ReplyAction="http://tempuri.org/IServiceContract/DeleteUserResponse")]
        void DeleteUser(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceContract/DeleteUser", ReplyAction="http://tempuri.org/IServiceContract/DeleteUserResponse")]
        System.Threading.Tasks.Task DeleteUserAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceContract/SearchBy", ReplyAction="http://tempuri.org/IServiceContract/SearchByResponse")]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ClientApplication.WcfService.UserDataContract))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ClientApplication.WcfService.Gender))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ClientApplication.WcfService.CountryVisa[]))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(ClientApplication.WcfService.CountryVisa))]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(int[]))]
        int[] SearchBy(object comparer, ClientApplication.WcfService.UserDataContract searchingUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceContract/SearchBy", ReplyAction="http://tempuri.org/IServiceContract/SearchByResponse")]
        System.Threading.Tasks.Task<int[]> SearchByAsync(object comparer, ClientApplication.WcfService.UserDataContract searchingUser);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceContractChannel : ClientApplication.WcfService.IServiceContract, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceContractClient : System.ServiceModel.ClientBase<ClientApplication.WcfService.IServiceContract>, ClientApplication.WcfService.IServiceContract {
        
        public ServiceContractClient() {
        }
        
        public ServiceContractClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceContractClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceContractClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceContractClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int AddUser(ClientApplication.WcfService.UserDataContract userData) {
            return base.Channel.AddUser(userData);
        }
        
        public System.Threading.Tasks.Task<int> AddUserAsync(ClientApplication.WcfService.UserDataContract userData) {
            return base.Channel.AddUserAsync(userData);
        }
        
        public void DeleteUser(int id) {
            base.Channel.DeleteUser(id);
        }
        
        public System.Threading.Tasks.Task DeleteUserAsync(int id) {
            return base.Channel.DeleteUserAsync(id);
        }
        
        public int[] SearchBy(object comparer, ClientApplication.WcfService.UserDataContract searchingUser) {
            return base.Channel.SearchBy(comparer, searchingUser);
        }
        
        public System.Threading.Tasks.Task<int[]> SearchByAsync(object comparer, ClientApplication.WcfService.UserDataContract searchingUser) {
            return base.Channel.SearchByAsync(comparer, searchingUser);
        }
    }
}