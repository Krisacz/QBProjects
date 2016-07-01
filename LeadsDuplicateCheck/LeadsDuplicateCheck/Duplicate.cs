namespace LeadsDuplicateCheck
{
    public class Duplicate
    {
        public AquariumLeadData AquariumLeadData { get; private set; }
        public ProclaimClaimData ProclaimClaimData { get; private set; }

        public Duplicate(AquariumLeadData aquariumLeadData, ProclaimClaimData proclaimClaimData)
        {
            AquariumLeadData = aquariumLeadData;
            ProclaimClaimData = proclaimClaimData;
        }
    }
}