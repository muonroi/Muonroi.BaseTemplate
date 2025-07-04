global using FluentValidation;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Muonroi.BaseTemplate.API.Application.Commands.Login;
global using Muonroi.BaseTemplate.API.Application.Commands.RefreshToken;
global using Muonroi.BaseTemplate.Data.Config;
global using Muonroi.BaseTemplate.Data.Persistence;
global using Muonroi.BuildingBlock.External.Common.Constants;
global using Muonroi.BuildingBlock.External.Common.Models.Requests.Login;
global using Muonroi.BuildingBlock.External.Common.Models.Responses.Login;
global using Muonroi.BuildingBlock.External.Controller;
global using Muonroi.BuildingBlock.External.Cors;
global using Muonroi.BuildingBlock.External.DI;
global using Muonroi.BuildingBlock.External.Entity.DataSample;
global using Muonroi.BuildingBlock.External.Interfaces;
global using Muonroi.BuildingBlock.External.Logging;
global using Muonroi.BuildingBlock.External.Response;
global using Muonroi.BuildingBlock.External.Services;
global using Muonroi.BuildingBlock.Internal.Services.Interfaces;
global using Serilog;
global using System.Reflection;
global using Muonroi.BaseTemplate.API.Services;
global using Muonroi.BaseTemplate.API.Infrastructures;
global using Muonroi.BuildingBlock.External;
global using Muonroi.BuildingBlock.External.Entity;
