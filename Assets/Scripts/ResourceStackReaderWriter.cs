using Mirror;

public static class ResourceStackReaderWriter
{ // This is used to transport ResourceStack between client and server.

        public static void WriteResourceStack(this NetworkWriter writer, ResourceStack resourceStack)
        {
            writer.WriteInt16((short)resourceStack.resourceType); // Todo: Test if this works.
            writer.WriteInt32(resourceStack.amount);
        }
     
        public static ResourceStack ReadResourceStack(this NetworkReader reader)
        {
            return new ResourceStack((ResourceType)reader.ReadInt16(), reader.ReadInt32());
        }
}