//Concesso in licenza a norma dell'EUPL, versione 1.2. 2019
using System.ComponentModel;

namespace Meti.Domain.ValueObjects
{
    public enum HealthRiskType
    {
        [Description("Patologie cardiache (solo cuore)")]
        HeartDisease = 1,
        [Description("Ipertensione")]
        Hypertension = 2,
        [Description("Patologie vascolari (sangue, vasi, midollo, milza, sistema linfatico)")]
        VascularPathologies = 3,
        [Description("Patologie respiratorie (polmoni, bronchi, trachea sotto la laringe)")]
        RespiratoryDisorders = 4,
        [Description("O.O.N.G.L")]
        Oongl = 5,
        [Description("Apparato GI superiore (esofago, stomaco, duodeno, albero biliare, pancreas)")]
        UpperGIApparatus = 6,
        [Description("Apparato GI inferiore (intestino, ernie)")]
        LowerGIApparatus = 7,
        [Description("Patologie epatiche (solo fegato)")]
        LiverDisease = 8,
        [Description("Patologie renali (solo rene)")]
        KidneyDisease = 9,
        [Description("Patologie genito-urinarie (ureteri, vescica, uretra, prostata, genitali)")]
        GenitourinaryPathologies = 10,
        [Description("Sistema muscolo-scheletro-cute")]
        MusculoskeletalSystem = 11,
        [Description("Patologie sistema nervoso centrale e periferico (no demenza)")]
        CentralNervousSystemDisorders = 12,
        [Description("Patologie endocrine-metaboliche (include diabete, infezioni, sepsi, stati tossici)")]
        EndocrineMetabolicPathologies = 13,
        [Description("Patologie psichiatriche-comportamentali (include demenza, depressione, ansia, agitazione, psicosi)")]
        PsychiatricBehavioralDisorders = 14,
    }
}