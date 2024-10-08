﻿using courseProject.Core.IGenericRepository;
using courseProject.Core.Models;
using courseProject.Repository.Data;
using Microsoft.EntityFrameworkCore;


namespace courseProject.Repository.GenericRepository
{
    public class SkillRepository :GenericRepository1<Skills> , ISkillRepository
    {
        private readonly projectDbContext dbContext;

        public SkillRepository(projectDbContext dbContext) :base(dbContext) 
        {
            this.dbContext = dbContext;
        }


        // Adds a new skill to the database.
        public async Task addSkillOptionsAsync(Skills skill)
        {
            await dbContext.Set<Skills>().AddAsync(skill);
        }


        // Retrieves all skills 
        public async Task<IReadOnlyList<Skills>> GetAllSkillsAsync()
        {
            return await dbContext.Skills.ToListAsync();
        }

        // Adds a list of skills to an instructor by instructor ID
        public async Task AddListOfSkillsAsync(Guid instructorId, List<Guid> skills)
        {
            InstructorSkills instructorSkills = new InstructorSkills();
            foreach (var skill in skills)
            {
                instructorSkills.InstructorId = instructorId;
                instructorSkills.skillId = skill;
                await dbContext.InstructorSkills.AddAsync(instructorSkills);
                await dbContext.SaveChangesAsync();
            }
        }

        // Retrieves the names of all specified skills.
        public async Task<IReadOnlyList<string>> GetAllSkillsNameToInstructorAsync(List<Guid> skills)
        {
            List<string> allSkills = new List<string>();
            foreach (var skill in skills)
            {
                allSkills.Add((await dbContext.Skills.FirstOrDefaultAsync(x => x.Id == skill)).name);
            }
            return (IReadOnlyList<string>)allSkills;
        }


        // Removes a specified skill from an instructor's skill set.
        public async Task RemoveASkill(InstructorSkills instructorSkills)
        {
            dbContext.InstructorSkills.Remove(instructorSkills);
        }

        // get all skilss where instructor by instructor id soes not selected it 
        public async Task<IReadOnlyList<Skills>> getAllUnregisteredSkillsOfTheInstructor(Guid instructorId)
        {
            return await dbContext.Skills.Where(x => !x.instructorSkills.Any(y => y.InstructorId == instructorId)).ToListAsync();
        }


        // Retrieves all skills selected by an instructor.
        public async Task<IReadOnlyList<Skills>> getAllInstructorSkills(Guid instructorId)
        {
            return await dbContext.Skills.Where(x => x.instructorSkills.Any(y => y.InstructorId == instructorId)).ToListAsync();
        }

    }
}
