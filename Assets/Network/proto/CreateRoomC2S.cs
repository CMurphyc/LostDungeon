// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: CreateRoomC2S.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Room {

  /// <summary>Holder for reflection information generated from CreateRoomC2S.proto</summary>
  public static partial class CreateRoomC2SReflection {

    #region Descriptor
    /// <summary>File descriptor for CreateRoomC2S.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CreateRoomC2SReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChNDcmVhdGVSb29tQzJTLnByb3RvEgRyb29tIjAKDUNyZWF0ZVJvb21DMlMS",
            "DQoFb3duZXIYASABKAkSEAoIcm9vbXNpemUYAiABKAViBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Room.CreateRoomC2S), global::Room.CreateRoomC2S.Parser, new[]{ "Owner", "Roomsize" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class CreateRoomC2S : pb::IMessage<CreateRoomC2S> {
    private static readonly pb::MessageParser<CreateRoomC2S> _parser = new pb::MessageParser<CreateRoomC2S>(() => new CreateRoomC2S());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CreateRoomC2S> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Room.CreateRoomC2SReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CreateRoomC2S() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CreateRoomC2S(CreateRoomC2S other) : this() {
      owner_ = other.owner_;
      roomsize_ = other.roomsize_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CreateRoomC2S Clone() {
      return new CreateRoomC2S(this);
    }

    /// <summary>Field number for the "owner" field.</summary>
    public const int OwnerFieldNumber = 1;
    private string owner_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Owner {
      get { return owner_; }
      set {
        owner_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "roomsize" field.</summary>
    public const int RoomsizeFieldNumber = 2;
    private int roomsize_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Roomsize {
      get { return roomsize_; }
      set {
        roomsize_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CreateRoomC2S);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CreateRoomC2S other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Owner != other.Owner) return false;
      if (Roomsize != other.Roomsize) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Owner.Length != 0) hash ^= Owner.GetHashCode();
      if (Roomsize != 0) hash ^= Roomsize.GetHashCode();
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
      if (Owner.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Owner);
      }
      if (Roomsize != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Roomsize);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Owner.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Owner);
      }
      if (Roomsize != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Roomsize);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CreateRoomC2S other) {
      if (other == null) {
        return;
      }
      if (other.Owner.Length != 0) {
        Owner = other.Owner;
      }
      if (other.Roomsize != 0) {
        Roomsize = other.Roomsize;
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
          case 10: {
            Owner = input.ReadString();
            break;
          }
          case 16: {
            Roomsize = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
