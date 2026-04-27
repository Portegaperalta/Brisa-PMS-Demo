namespace BrisaPMS.Persistence.Entities
{
    public class AmenityRoomType
    {
        private AmenityRoomType() { }

        public Guid AmenityId { get; private set; }
        public Guid RoomTypeId { get; private set; }

        public static AmenityRoomType Create(Guid amenityId, Guid roomTypeId)
            => new AmenityRoomType { AmenityId = amenityId, RoomTypeId = roomTypeId };
       
    }
}