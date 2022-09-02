namespace SimaDat.Core
{
    public static class AutoMapperInitializer
    {
        private static bool _isInitialized = false;

        public static void Initialize()
        {
            if (_isInitialized == false)
            {
                // Mapper.Initialize(cfg =>
                // {
                //     cfg.CreateMap<SerializedLocation, Location>()
                //         .ForMember(d => d.Doors, opt => opt.Ignore());
                //     cfg.CreateMap<Location, SerializedLocation>(MemberList.Source)
                //         .ForMember(d => d.doors, opt => opt.Ignore())
                //         .ForMember(d => d.id, src => src.MapFrom(s => s.LocationId))
                //         .ForMember(d => d.doors, src => src.MapFrom(s =>
                //             s.Doors.Select(x => new string[] { SerializerBll.DirectionToCode(x.Direction), x.LocationToGoId.ToString() }).ToList()
                //         ))
                //         .ReverseMap();
                //
                //     cfg.CreateMap<string[], Location.Door>(MemberList.Source)
                //         .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
                //     });

                _isInitialized = true;
            }
        }
    }
}
