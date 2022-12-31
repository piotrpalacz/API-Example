namespace ExampleAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonObjectAttribute]
    [NotMapped]
    public partial class SearchBase
    {
        [JsonProperty(PropertyName ="totalHits")]
        public long TotalHits { get; set; }

        [JsonProperty(PropertyName ="currentPage")]
        public long CurrentPage { get; set; }

        [JsonProperty("totalPages")]
        public long TotalPages { get; set; }

        [JsonProperty("pageList")]
        public long[]? PageList { get; set; }

        [JsonProperty("foodSearchCriteria")]
        public FoodSearchCriteria? FoodSearchCriteria { get; set; }

        [JsonProperty("foods")]
        public FoodElement[]? Foods { get; set; }
    }

    public partial class FoodSearchCriteria
    {
        [JsonProperty("dataType")]
        public DataTypeElement[]? DataType { get; set; }

        [JsonProperty("query")]
        public string? Query { get; set; }

        [JsonProperty("generalSearchInput")]
        public string? GeneralSearchInput { get; set; }

        [JsonProperty("pageNumber")]
        public long PageNumber { get; set; }

        [JsonProperty("sortBy")]
        public string? SortBy { get; set; }

        [JsonProperty("sortOrder")]
        public string? SortOrder { get; set; }

        [JsonProperty("numberOfResultsPerPage")]
        public long NumberOfResultsPerPage { get; set; }

        [JsonProperty("pageSize")]
        public long PageSize { get; set; }

        [JsonProperty("foodTypes")]
        public DataTypeElement[]? FoodTypes { get; set; }
    }

    public partial class FoodElement
    {
        [JsonProperty("fdcId")]
        public long FdcId { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("dataType")]
        public DataTypeElement DataType { get; set; }

        [JsonProperty("ndbNumber")]
        public long NdbNumber { get; set; }

        [JsonProperty("publishedDate")]
        public DateTimeOffset PublishedDate { get; set; }

        [JsonProperty("foodCategory")]
        [JsonConverter(typeof(FoodCategoryConverter))]
        public FoodCategory FoodCategory { get; set; }

        [JsonProperty("mostRecentAcquisitionDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? MostRecentAcquisitionDate { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("foodNutrients")]
        public FoodNutrient[]? FoodNutrients { get; set; }
    }

    public partial class FoodNutrient
    {
        [JsonProperty("nutrientId")]
        public long NutrientId { get; set; }

        [JsonProperty("nutrientName")]
        public string? NutrientName { get; set; }

        [JsonProperty("nutrientNumber")]
        public string? NutrientNumber { get; set; }

        [JsonProperty("unitName", NullValueHandling = NullValueHandling.Ignore)]
        public UnitName? UnitName { get; set; }

        [JsonProperty("derivationCode", NullValueHandling = NullValueHandling.Ignore)]
        public DerivationCode? DerivationCode { get; set; }

        [JsonProperty("derivationDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string? DerivationDescription { get; set; }

        [JsonProperty("derivationId", NullValueHandling = NullValueHandling.Ignore)]
        public long? DerivationId { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public double? Value { get; set; }

        [JsonProperty("foodNutrientSourceId", NullValueHandling = NullValueHandling.Ignore)]
        public long? FoodNutrientSourceId { get; set; }

        [JsonProperty("foodNutrientSourceCode", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long? FoodNutrientSourceCode { get; set; }

        [JsonProperty("foodNutrientSourceDescription", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(FoodNutrientSourceDescriptionConverter))]
        public FoodNutrientSourceDescription? FoodNutrientSourceDescription { get; set; }

        [JsonProperty("rank")]
        public long Rank { get; set; }

        [JsonProperty("indentLevel")]
        public long IndentLevel { get; set; }

        [JsonProperty("foodNutrientId")]
        public long FoodNutrientId { get; set; }

        [JsonProperty("dataPoints", NullValueHandling = NullValueHandling.Ignore)]
        public long? DataPoints { get; set; }

        [JsonProperty("min", NullValueHandling = NullValueHandling.Ignore)]
        public double? Min { get; set; }

        [JsonProperty("max", NullValueHandling = NullValueHandling.Ignore)]
        public double? Max { get; set; }

        [JsonProperty("median", NullValueHandling = NullValueHandling.Ignore)]
        public double? Median { get; set; }
    }

    public enum DataTypeElement { Foundation, SrLegacy };

    public enum FoodCategory { BakedProducts, DairyAndEggProducts, NutAndSeedProducts, FruitsAndFruitJuices, SoupsSaucesAndGravies, VegetablesAndVegetableProducts, CerealGrainsAndPasta };

    public enum DerivationCode { A, As, Bffn, Bfnn, Bfzn, Cazn, Flc, Lc, Nc, Nr, T, Z };

    public enum FoodNutrientSourceDescription { AnalyticalOrDerivedFromAnalytical, AssumedZero, CalculatedFromNutrientLabelByNdl, CalculatedOrImputed };

    public enum UnitName { G, Iu, KJ, Kcal, Mg, Ug };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                DataTypeElementConverter.Singleton,
                FoodCategoryConverter.Singleton,
                DerivationCodeConverter.Singleton,
                FoodNutrientSourceDescriptionConverter.Singleton,
                UnitNameConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class DataTypeElementConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(DataTypeElement) || t == typeof(DataTypeElement?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Foundation":
                    return DataTypeElement.Foundation;
                case "SR Legacy":
                    return DataTypeElement.SrLegacy;
            }
            throw new Exception("Cannot unmarshal type DataTypeElement");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (DataTypeElement)untypedValue;
            switch (value)
            {
                case DataTypeElement.Foundation:
                    serializer.Serialize(writer, "Foundation");
                    return;
                case DataTypeElement.SrLegacy:
                    serializer.Serialize(writer, "SR Legacy");
                    return;
            }
            throw new Exception("Cannot marshal type DataTypeElement");
        }

        public static readonly DataTypeElementConverter Singleton = new DataTypeElementConverter();
    }

    internal class FoodCategoryConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(FoodCategory) || t == typeof(FoodCategory?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Baked Products":
                    return FoodCategory.BakedProducts;
                case "Dairy and Egg Products":
                    return FoodCategory.DairyAndEggProducts;
                case "Nut and Seed Products":
                    return FoodCategory.NutAndSeedProducts;
                case "Fruits and Fruit Juices":
                    return FoodCategory.FruitsAndFruitJuices;
                case "Soups, Sauces, and Gravies":
                    return FoodCategory.SoupsSaucesAndGravies;
                case "Vegetables and Vegetable Products":
                    return FoodCategory.VegetablesAndVegetableProducts;
                case "Cereal Grains and Pasta":
                    return FoodCategory.CerealGrainsAndPasta;
            }
            throw new Exception("Cannot unmarshal type FoodCategory");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (FoodCategory)untypedValue;
            switch (value)
            {
                case FoodCategory.BakedProducts:
                    serializer.Serialize(writer, "Baked Products");
                    return;
                case FoodCategory.DairyAndEggProducts:
                    serializer.Serialize(writer, "Dairy and Egg Products");
                    return;
                case FoodCategory.NutAndSeedProducts:
                    serializer.Serialize(writer, "Nut and Seed Products");
                    return;
                case FoodCategory.FruitsAndFruitJuices:
                    serializer.Serialize(writer, "Fruits and Fruit Juices");
                    return;
                case FoodCategory.SoupsSaucesAndGravies:
                    serializer.Serialize(writer, "Soups, Sauces, and Gravies");
                    return;
                case FoodCategory.VegetablesAndVegetableProducts:
                    serializer.Serialize(writer, "Vegetables and Vegetable Products");
                    return;
                case FoodCategory.CerealGrainsAndPasta:
                    serializer.Serialize(writer, "Cereal Grains and Pasta");
                    return;
            }
            throw new Exception("Cannot marshal type FoodCategory");
        }

        public static readonly FoodCategoryConverter Singleton = new FoodCategoryConverter();
    }

    internal class DerivationCodeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(DerivationCode) || t == typeof(DerivationCode?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "A":
                    return DerivationCode.A;
                case "AS":
                    return DerivationCode.As;
                case "BFFN":
                    return DerivationCode.Bffn;
                case "BFNN":
                    return DerivationCode.Bfnn;
                case "BFZN":
                    return DerivationCode.Bfzn;
                case "CAZN":
                    return DerivationCode.Cazn;
                case "FLC":
                    return DerivationCode.Flc;
                case "LC":
                    return DerivationCode.Lc;
                case "NC":
                    return DerivationCode.Nc;
                case "NR":
                    return DerivationCode.Nr;
                case "T":
                    return DerivationCode.T;
                case "Z":
                    return DerivationCode.Z;
            }
            throw new Exception("Cannot unmarshal type DerivationCode");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (DerivationCode)untypedValue;
            switch (value)
            {
                case DerivationCode.A:
                    serializer.Serialize(writer, "A");
                    return;
                case DerivationCode.As:
                    serializer.Serialize(writer, "AS");
                    return;
                case DerivationCode.Bffn:
                    serializer.Serialize(writer, "BFFN");
                    return;
                case DerivationCode.Bfnn:
                    serializer.Serialize(writer, "BFNN");
                    return;
                case DerivationCode.Bfzn:
                    serializer.Serialize(writer, "BFZN");
                    return;
                case DerivationCode.Cazn:
                    serializer.Serialize(writer, "CAZN");
                    return;
                case DerivationCode.Flc:
                    serializer.Serialize(writer, "FLC");
                    return;
                case DerivationCode.Lc:
                    serializer.Serialize(writer, "LC");
                    return;
                case DerivationCode.Nc:
                    serializer.Serialize(writer, "NC");
                    return;
                case DerivationCode.Nr:
                    serializer.Serialize(writer, "NR");
                    return;
                case DerivationCode.T:
                    serializer.Serialize(writer, "T");
                    return;
                case DerivationCode.Z:
                    serializer.Serialize(writer, "Z");
                    return;
            }
            throw new Exception("Cannot marshal type DerivationCode");
        }

        public static readonly DerivationCodeConverter Singleton = new DerivationCodeConverter();
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class FoodNutrientSourceDescriptionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(FoodNutrientSourceDescription) || t == typeof(FoodNutrientSourceDescription?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Analytical or derived from analytical":
                    return FoodNutrientSourceDescription.AnalyticalOrDerivedFromAnalytical;
                case "Assumed zero":
                    return FoodNutrientSourceDescription.AssumedZero;
                case "Calculated from nutrient label by NDL":
                    return FoodNutrientSourceDescription.CalculatedFromNutrientLabelByNdl;
                case "Calculated or imputed":
                    return FoodNutrientSourceDescription.CalculatedOrImputed;
            }
            throw new Exception("Cannot unmarshal type FoodNutrientSourceDescription");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (FoodNutrientSourceDescription)untypedValue;
            switch (value)
            {
                case FoodNutrientSourceDescription.AnalyticalOrDerivedFromAnalytical:
                    serializer.Serialize(writer, "Analytical or derived from analytical");
                    return;
                case FoodNutrientSourceDescription.AssumedZero:
                    serializer.Serialize(writer, "Assumed zero");
                    return;
                case FoodNutrientSourceDescription.CalculatedFromNutrientLabelByNdl:
                    serializer.Serialize(writer, "Calculated from nutrient label by NDL");
                    return;
                case FoodNutrientSourceDescription.CalculatedOrImputed:
                    serializer.Serialize(writer, "Calculated or imputed");
                    return;
            }
            throw new Exception("Cannot marshal type FoodNutrientSourceDescription");
        }

        public static readonly FoodNutrientSourceDescriptionConverter Singleton = new FoodNutrientSourceDescriptionConverter();
    }

    internal class UnitNameConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(UnitName) || t == typeof(UnitName?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "G":
                    return UnitName.G;
                case "IU":
                    return UnitName.Iu;
                case "KCAL":
                    return UnitName.Kcal;
                case "MG":
                    return UnitName.Mg;
                case "UG":
                    return UnitName.Ug;
                case "kJ":
                    return UnitName.KJ;
            }
            throw new Exception("Cannot unmarshal type UnitName");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (UnitName)untypedValue;
            switch (value)
            {
                case UnitName.G:
                    serializer.Serialize(writer, "G");
                    return;
                case UnitName.Iu:
                    serializer.Serialize(writer, "IU");
                    return;
                case UnitName.Kcal:
                    serializer.Serialize(writer, "KCAL");
                    return;
                case UnitName.Mg:
                    serializer.Serialize(writer, "MG");
                    return;
                case UnitName.Ug:
                    serializer.Serialize(writer, "UG");
                    return;
                case UnitName.KJ:
                    serializer.Serialize(writer, "kJ");
                    return;
            }
            throw new Exception("Cannot marshal type UnitName");
        }

        public static readonly UnitNameConverter Singleton = new UnitNameConverter();
    }
}
