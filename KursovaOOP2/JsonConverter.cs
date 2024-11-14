using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace KursovaOOP2
{
    internal class ShapeConverter : JsonConverter
    {
        
            public override bool CanConvert(Type objectType)
            {
                return typeof(Shape).IsAssignableFrom(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                JObject jsonObject = JObject.Load(reader);
                string typeName = jsonObject["Type"].Value<string>();

                Shape shape;
                switch (typeName)
                {
                    case "Circle":
                        shape = new Circle();
                        break;
                    case "Triangle":
                        shape = new Triangle();
                        break;
                    case "Square":
                        shape = new Square();
                        break;
                    case "Line":
                        shape = new Line();
                        break;
                    default:
                        throw new NotSupportedException($"Unsupported shape type: {typeName}");
                }

                serializer.Populate(jsonObject.CreateReader(), shape);
                return shape;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                Shape shape = (Shape)value;

                JObject jsonObject = new JObject();
                jsonObject.Add("Type", JToken.FromObject(shape.GetType().Name));
                serializer.Serialize(writer, shape);
            }
        }
    
}
