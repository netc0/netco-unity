using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf.Meta;
using ProtoBuf;
using System.IO;

public class ProtobufTest : MonoBehaviour {

    byte[] encode(Entity entity) {
        byte[] data = null;
        using (MemoryStream stream = new MemoryStream()) {
            //Serializer.Serialize<Entity>(stream, entity);
            Serializer.Serialize(stream, entity);
            data = stream.ToArray();
        }
        return data;
    }

    void decode(byte[] data) {
        Debug.Log(data.ToStringx());
        using (MemoryStream stream = new MemoryStream(data)) {
            var ent = Serializer.Deserialize<Entity>(stream);
            if (ent != null) {
                System.Console.WriteLine(ent.Foo);
            }
            Debug.LogError(ent.Foo);
        }
    }


    private void Start() {
        Entity entity = new Entity();
        entity.Foo = "hello!";
        byte[] data = encode(entity);
        decode(data);

    }
}

[ProtoContract]
class Entity {
    [ProtoMember(1)]
    public string Foo { get; set; }
}