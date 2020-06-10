using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YukiNet.PackageParser.PackageImplementaions
{
    [PackageTypAttribute(CommunicationPackageValues.CHARACTER_CREATION_REQUEST)]
    public class CharacterCreationRequestPackage : PackageBase
    {
        public string CharName { get; set; }
        public string UmaRecipe { get; set; }
        public int ErrorMsg { get; set; }
        public CharacterCreationRequestPackage() : base(CommunicationPackageValues.CHARACTER_CREATION_REQUEST)
        {
        }

        public override void DeserialzeFromStream(BinaryReader reader)
        {
            //var namesize = reader.ReadInt32();
            //CharName = Encoding.UTF8.GetString(reader.ReadBytes(namesize));
            //var umasize = reader.ReadInt32();
            //UmaRecipe = Encoding.UTF8.GetString(reader.ReadBytes(umasize));
            CharName = reader.ReadString();
            UmaRecipe = reader.ReadString();

        }

        public override void SerialzeToStream(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(CharName);
            writer.Write(UmaRecipe);
            //var charnameBytes = Encoding.UTF8.GetBytes(CharName);
            //writer.Write(charnameBytes.Length);
            //writer.Write(charnameBytes, 0, charnameBytes.Length);

            //var umaRecipeBytes = Encoding.UTF8.GetBytes(UmaRecipe);
            //writer.Write(umaRecipeBytes.Length);
            //writer.Write(umaRecipeBytes, 0, umaRecipeBytes.Length);
        }
    }

    [PackageTypAttribute(CommunicationPackageValues.CHARACTER_CREATION_RESPONSE)]
    public class CharacterResponsePackage : PackageBase
    {
        public bool IsValid { get; set; }
        public int ErrorMsg { get; set; }

        public CharacterResponsePackage() : base(CommunicationPackageValues.CHARACTER_CREATION_RESPONSE)
        {
        }

        public override void DeserialzeFromStream(BinaryReader reader)
        {
            IsValid = reader.ReadBoolean();

        }

        public override void SerialzeToStream(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(IsValid);

        }
    }



    [PackageTypAttribute(CommunicationPackageValues.CHARACTER_SELECTION_REQUEST)]
    public class CharacterSelectionRequestPackage : PackageBase
    {
        public bool IsValid { get; set; }
        public int ErrorMsg { get; set; }

        public CharacterSelectionRequestPackage() : base(CommunicationPackageValues.CHARACTER_SELECTION_REQUEST)
        {
        }

        public override void DeserialzeFromStream(BinaryReader reader)
        {
            IsValid = reader.ReadBoolean();

        }

        public override void SerialzeToStream(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(IsValid);

        }
    }


    [PackageTypAttribute(CommunicationPackageValues.CHARACTER_SELECTION_RESPONSE)]
    public class CharacterSelectionResponsePackage : PackageBase
    {
        public bool IsValid { get; set; }
        public int ErrorMsg { get; set; }

        /*
        public int CharID { get; set; }
        public string CharName { get; set; }
        
        */

        public IEnumerable<CharData> Characters { get; set; }

        public CharacterSelectionResponsePackage() : base(CommunicationPackageValues.CHARACTER_SELECTION_RESPONSE)
        {
            Characters = new List<CharData>();
        }

        public override void DeserialzeFromStream(BinaryReader reader)
        {
            IsValid = reader.ReadBoolean();
            ErrorMsg = reader.ReadInt32();
            // Characters = reader.ReadString();

            var count = reader.ReadByte();

            var charlist = new List<CharData>();
            Characters = charlist;

            for (int i = 0; i < count; i++)
            {
                charlist.Add(new CharData
                {
                    CharId = reader.ReadInt32(),
                    CharName = reader.ReadString(),
                    UmaData = reader.ReadString(),
                });
            }
        }

        public override void SerialzeToStream(BinaryWriter writer)
        {
            writer.Write(ID);
            writer.Write(IsValid);
            writer.Write(ErrorMsg);

            writer.Write((byte)Characters.Count());

            foreach (var character in Characters)
            {
                writer.Write(character.CharId);
                writer.Write(character.CharName);
                writer.Write(character.UmaData);
            }
        } 
    }

    public class CharData
    {
        public string CharName { get; set; }
        public int CharId { get; set; }
        public string UmaData { get; set; }
    }
}