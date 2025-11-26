# FoodBev Platform - API Endpoints Testing Guide

## Base URL
- Development: `http://localhost:5259/api/v1`
- Swagger UI: `http://localhost:5259/swagger`

## Authentication
All endpoints (except public ones) require JWT Bearer token in Authorization header:
```
Authorization: Bearer <token>
```

---

## ✅ AUTHENTICATION ENDPOINTS

### 1. Register User
- **POST** `/auth/register`
- **Body:**
  ```json
  {
    "email": "user@example.com",
    "password": "Password123!",
    "userType": 0  // 0=Candidate, 1=Employer, 2=Admin
  }
  ```
- **Response:** `{ token, userID, email, userType, isAuthenticated }`

### 2. Login
- **POST** `/auth/login`
- **Body:**
  ```json
  {
    "email": "user@example.com",
    "password": "Password123!"
  }
  ```
- **Response:** `{ token, userID, email, userType, isAuthenticated }`

### 3. Validate Token
- **POST** `/auth/validateToken`
- **Headers:** `Authorization: Bearer <token>`
- **Response:** `{ isAuthenticated, email, userType }`

---

## ✅ CANDIDATE ENDPOINTS

### Profile Management

#### Get Profile
- **GET** `/candidate/profile`
- **Auth:** Candidate role required
- **Response:** Full candidate profile

#### Update Profile
- **PUT** `/candidate/profile`
- **Auth:** Candidate role required
- **Body:** UpdateCandidateProfileDto (all fields optional except OFO_Code)

### Document Management

#### Upload ID Document
- **POST** `/candidate/documents`
- **Auth:** Candidate role required
- **Content-Type:** `multipart/form-data`
- **Body:** `file` (PDF, JPG, JPEG, PNG, max 5MB)
- **Response:** `{ message, documentUrl, fileName }`

#### Get Documents
- **GET** `/candidate/documents`
- **Auth:** Candidate role required
- **Response:** `{ documents: [{ type, url, uploaded }] }`

#### Delete Document
- **DELETE** `/candidate/documents/{docType}` (e.g., "id" or "id_document")
- **Auth:** Candidate role required

### Job Discovery

#### Get Matching Jobs
- **GET** `/candidate/jobs`
- **Auth:** Candidate role required
- **Response:** Array of jobs matching candidate's OFO code
- **Filters:** Automatically filters by candidate's OFO code

### Applications

#### Apply to Job
- **POST** `/candidate/applications/{jobId}`
- **Auth:** Candidate role required
- **Response:** Application summary

#### Get My Applications
- **GET** `/candidate/applications`
- **Auth:** Candidate role required
- **Response:** Array of application summaries

#### Respond to Interview
- **PUT** `/applications/{applicationId}/interview-response`
- **Auth:** Candidate role required
- **Body:**
  ```json
  {
    "response": 1  // 0=None, 1=Accepted, 2=Declined
  }
  ```

### Saved Jobs

#### Save Job
- **POST** `/candidate/saved-jobs/{jobId}`
- **Auth:** Candidate role required

#### Get Saved Jobs
- **GET** `/candidate/saved-jobs`
- **Auth:** Candidate role required
- **Response:** Array of saved jobs

---

## ✅ EMPLOYER ENDPOINTS

### Profile Management

#### Get Profile
- **GET** `/employer/profile`
- **Auth:** Employer role required

#### Update Profile
- **PUT** `/employer/profile`
- **Auth:** Employer role required

### Job Management

#### Get My Jobs
- **GET** `/employer/jobs`
- **Auth:** Employer role required
- **Response:** Array of jobs posted by employer

#### Create Job
- **POST** `/jobs`
- **Auth:** Employer role required
- **Body:**
  ```json
  {
    "jobTitle": "Software Developer",
    "jobDescription": "Description here",
    "ofO_Code_Required": "123456",
    "preferredProvince": "Gauteng",  // Optional
    "isBursary": false,
    "applicationDeadline": "2025-12-31T00:00:00Z"
  }
  ```

#### Update Job
- **PUT** `/jobs/{jobId}`
- **Auth:** Employer role required (must own the job)

#### Delete Job
- **DELETE** `/jobs/{jobId}`
- **Auth:** Employer role required (must own the job)

### Applicant Management

#### Get Applicants for Job
- **GET** `/jobs/{jobId}/applicants?ofoCode=X&employmentStatus=Y&province=Z`
- **Auth:** Employer role required (must own the job)
- **Query Params (all optional):**
  - `ofoCode`: Filter by candidate's OFO code
  - `employmentStatus`: Filter by employment status (e.g., "Unemployed")
  - `province`: Filter by candidate's province
- **Response:** Array of filtered application summaries

#### Get Applicant Profile
- **GET** `/jobs/{jobId}/applicants/{candidateId}`
- **Auth:** Employer role required
- **Response:** `{ application, candidate }`

#### Update Application Status
- **PUT** `/jobs/{jobId}/applicants/{candidateId}/status`
- **Auth:** Employer role required
- **Body:**
  ```json
  {
    "status": "Shortlisted"  // Applied, Shortlisted, InterviewScheduled, etc.
  }
  ```

#### Schedule Interview
- **PUT** `/applications/{applicationId}/schedule-interview`
- **Auth:** Employer role required
- **Body:**
  ```json
  {
    "interviewDate": "2025-12-15T10:00:00Z",
    "interviewVenue": "123 Main St, Johannesburg"
  }
  ```

#### Update Application Status (Direct)
- **PUT** `/applications/{applicationId}/status`
- **Auth:** Employer role required
- **Body:**
  ```json
  {
    "status": "Hired"  // or "Rejected", "Shortlisted", etc.
  }
  ```

### Logo Upload

#### Upload Logo
- **POST** `/employer/logo`
- **Auth:** Employer role required
- **Content-Type:** `multipart/form-data`
- **Body:** `file` (image file)
- **Response:** `{ LogoUrl }`

---

## ✅ PUBLIC ENDPOINTS

### Job Search
- **GET** `/jobs/search?query=X&ofO_Code=Y&isBursary=true&employerID=Z`
- **Auth:** None required
- **Query Params (all optional):**
  - `query`: Search in job title/description
  - `ofO_Code`: Filter by OFO code
  - `isBursary`: Filter by bursary status
  - `employerID`: Filter by employer

### Get Job Details
- **GET** `/jobs/{id}`
- **Auth:** None required
- **Response:** Full job details

### Public Employer Profile
- **GET** `/employer/{employerId}/public-profile`
- **Auth:** None required

---

## ✅ ADMIN ENDPOINTS

### Dashboard Statistics
- **GET** `/admin/stats`
- **Auth:** Admin role required
- **Response:**
  ```json
  {
    "totalStudents": 150,
    "totalApplications": 320,
    "activeStudents24h": 45,
    "fundedCompanies": 28
  }
  ```

### Demographics
- **GET** `/admin/demographics`
- **Auth:** Admin role required
- **Response:**
  ```json
  [
    { "province": "Gauteng", "count": 1200 },
    { "province": "Western Cape", "count": 800 }
  ]
  ```

### Recent Activity
- **GET** `/admin/recent-activity?limit=50`
- **Auth:** Admin role required
- **Response:** Array of activity items

---

## 🧪 TESTING CHECKLIST

### Authentication Flow
- [ ] Register as Candidate
- [ ] Register as Employer
- [ ] Login with credentials
- [ ] Validate token

### Candidate Flow
- [ ] Get profile (should be mostly empty initially)
- [ ] Update profile with all required fields
- [ ] Upload ID document
- [ ] Get matching jobs (should show jobs matching OFO code)
- [ ] Apply to a job
- [ ] Get my applications
- [ ] Save a job
- [ ] Get saved jobs
- [ ] Respond to interview invitation (if scheduled)

### Employer Flow
- [ ] Get employer profile
- [ ] Update employer profile
- [ ] Create a job posting
- [ ] Get my jobs
- [ ] Get applicants for a job
- [ ] Filter applicants (OFO, EmploymentStatus, Province)
- [ ] Shortlist an applicant
- [ ] Schedule interview
- [ ] Update application status to Hired/Rejected
- [ ] Upload company logo

### Admin Flow
- [ ] Get dashboard stats
- [ ] Get demographics
- [ ] Get recent activity

### Public Endpoints
- [ ] Search jobs (no auth)
- [ ] Get job details (no auth)
- [ ] Get public employer profile (no auth)

---

## 📝 NOTES

1. **OFO Code Matching:** Jobs are only shown to candidates whose OFO code matches the job's required OFO code
2. **Profile Completeness:** Candidates should complete their profile (especially OFO code) to see matching jobs
3. **Interview Workflow:** 
   - Employer schedules interview → Status becomes "InterviewScheduled"
   - Candidate accepts/declines → Status becomes "InterviewAccepted" or "InterviewDeclined"
4. **File Uploads:** 
   - Max file size: 5MB
   - Allowed types: PDF, JPG, JPEG, PNG
   - Files stored in `wwwroot/uploads/`

---

## 🔧 TROUBLESHOOTING

### Common Issues:
1. **401 Unauthorized:** Check token is valid and included in Authorization header
2. **403 Forbidden:** Check user has correct role (Candidate/Employer/Admin)
3. **404 Not Found:** Verify endpoint URL and resource ID
4. **400 Bad Request:** Check request body matches DTO structure
5. **File upload fails:** Verify file size < 5MB and correct file type

