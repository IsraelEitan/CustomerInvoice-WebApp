using AutoMapper;
using CustomerInvoice_WebApp.Dtos;
using CustomerInvoice_WebApp.Enums;
using CustomerInvoice_WebApp.Exceptions;
using CustomerInvoice_WebApp.Models;

namespace CustomerInvoice_WebApp.Mappers.Profiles
{
    internal sealed class InvoicesProfile : Profile
    {
        public InvoicesProfile()
        {
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<CreateInvoiceDto, Invoice>();
            CreateMap<UpdateInvoiceDto, Invoice>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
          
            CreateMap<string, InvoiceStatus>().ConvertUsing(new StringToEnumConverter<InvoiceStatus>());
        }
    }

    public class StringToEnumConverter<TEnum> : ITypeConverter<string, TEnum> where TEnum : struct
    {
        public TEnum Convert(string source, TEnum destination, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new InvalidInputException("Cannot convert null or empty string to enum.");
            }

            if (Enum.TryParse<TEnum>(source, true, out var enumResult))
            {
                return enumResult;
            }
            else
            {
                throw new InvalidInputException($"Invalid value for enum {typeof(TEnum).Name}: {source}");
            }
        }
    }
}
