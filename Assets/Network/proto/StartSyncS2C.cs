// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: StartSyncS2C.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from StartSyncS2C.proto</summary>
public static partial class StartSyncS2CReflection {

  #region Descriptor
  /// <summary>File descriptor for StartSyncS2C.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static StartSyncS2CReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "ChJTdGFydFN5bmNTMkMucHJvdG8iLgoMU3RhcnRTeW5jUzJDEg0KBWVycm9y",
          "GAEgASgFEg8KB3N1Y2NlZWQYAiABKAhiBnByb3RvMw=="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::StartSyncS2C), global::StartSyncS2C.Parser, new[]{ "Error", "Succeed" }, null, null, null)
        }));
  }
  #endregion

}
#region Messages
public sealed partial class StartSyncS2C : pb::IMessage<StartSyncS2C> {
  private static readonly pb::MessageParser<StartSyncS2C> _parser = new pb::MessageParser<StartSyncS2C>(() => new StartSyncS2C());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pb::MessageParser<StartSyncS2C> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::StartSyncS2CReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public StartSyncS2C() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public StartSyncS2C(StartSyncS2C other) : this() {
    error_ = other.error_;
    succeed_ = other.succeed_;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public StartSyncS2C Clone() {
    return new StartSyncS2C(this);
  }

  /// <summary>Field number for the "error" field.</summary>
  public const int ErrorFieldNumber = 1;
  private int error_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int Error {
    get { return error_; }
    set {
      error_ = value;
    }
  }

  /// <summary>Field number for the "succeed" field.</summary>
  public const int SucceedFieldNumber = 2;
  private bool succeed_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Succeed {
    get { return succeed_; }
    set {
      succeed_ = value;
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override bool Equals(object other) {
    return Equals(other as StartSyncS2C);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Equals(StartSyncS2C other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Error != other.Error) return false;
    if (Succeed != other.Succeed) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override int GetHashCode() {
    int hash = 1;
    if (Error != 0) hash ^= Error.GetHashCode();
    if (Succeed != false) hash ^= Succeed.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void WriteTo(pb::CodedOutputStream output) {
    if (Error != 0) {
      output.WriteRawTag(8);
      output.WriteInt32(Error);
    }
    if (Succeed != false) {
      output.WriteRawTag(16);
      output.WriteBool(Succeed);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int CalculateSize() {
    int size = 0;
    if (Error != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Error);
    }
    if (Succeed != false) {
      size += 1 + 1;
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(StartSyncS2C other) {
    if (other == null) {
      return;
    }
    if (other.Error != 0) {
      Error = other.Error;
    }
    if (other.Succeed != false) {
      Succeed = other.Succeed;
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(pb::CodedInputStream input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 8: {
          Error = input.ReadInt32();
          break;
        }
        case 16: {
          Succeed = input.ReadBool();
          break;
        }
      }
    }
  }

}

#endregion


#endregion Designer generated code