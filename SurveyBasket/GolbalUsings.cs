global using System.Text;
global using System.Reflection;
global using System.Security.Claims;
global using System.IdentityModel.Tokens.Jwt;

global using Mapster;
global using MapsterMapper;
global using FluentValidation;

global using Microsoft.Extensions.Options;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.AspNetCore.Authentication.JwtBearer;

global using SurveyBasket.Entities;
global using SurveyBasket.Services;
global using SurveyBasket.Persistence;
global using SurveyBasket.Contracts.Polls;
global using SurveyBasket.Contracts.Authentication;
global using SurveyBasket.Abstractions;
global using SurveyBasket.Errors;