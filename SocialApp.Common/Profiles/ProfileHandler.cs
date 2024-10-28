using SocialApp.Common.Profiles.Create;
using SocialApp.Data.Models;
using SocialApp.Data.Repositories;

namespace SocialApp.Common.Profiles;

public interface IProfileHandler
{
    Task<ProfileDto> Get(string email, CancellationToken cancellationToken);
    Task<ProfileDto> GetByUsername(string username, CancellationToken cancellationToken);
    Task CreateProfile(CreateProfileRequest request, CancellationToken cancellationToken);
    Task Update(ProfileDto profile, CancellationToken cancellationToken);
}

public class ProfileHandler : IProfileHandler
{
    private readonly IProfileRepository _profileRepository;

    public ProfileHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<ProfileDto> Get(string email, CancellationToken cancellationToken) =>
        (await _profileRepository.Get(email, cancellationToken)).ToDto();

    public async Task<ProfileDto> GetByUsername(string email, CancellationToken cancellationToken) =>
        (await _profileRepository.GetByUsername(email, cancellationToken)).ToDto();

    public async Task CreateProfile(CreateProfileRequest request, CancellationToken cancellationToken)
    {
        var model = new ProfileModel
        {
            Friends = [],
            Id = Guid.NewGuid(),
            Email = request.Email,
            CreatedAt = DateTime.Now,
            Username = request.Username,
            LastName = request.LastName,
            FirstName = request.FirstName,
            UserId = request.UserId
        };

        await _profileRepository.Save(model, cancellationToken);
    }

    public async Task Update(ProfileDto profile, CancellationToken cancellationToken)
    {
        var model = await _profileRepository.GetById(profile.UserId, cancellationToken);

        model.Email = profile.Email;
        model.FirstName = profile.FirstName;
        model.LastName = profile.LastName;
        model.ProfilePicture = profile.ProfilePicture;

        await _profileRepository.Update(model, cancellationToken);
    }
}