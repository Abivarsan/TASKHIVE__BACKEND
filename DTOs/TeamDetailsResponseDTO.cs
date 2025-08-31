namespace TASKHIVE.DTOs
{
    public class TeamDetailsResponseDTO
    {
       
        public string TeamName { get; set; }
        public string ProjectName { get; set; }
        public string ProjectStatus { get; set; }
        public List<GetTeamDetailsDTO> Developers { get; set; }
    }
}
