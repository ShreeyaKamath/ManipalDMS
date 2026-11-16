-- This repository intentionally does not include privileged database creation scripts.
-- Provision production SQL logins and role memberships through your enterprise access-management process.
-- Use the least-privilege principle, secure secret storage, and environment-specific deployment policies.

-- Example secure pattern for a DBA-managed deployment:
-- 1. Create a dedicated application login.
-- 2. Grant only the minimum database permissions required for the application.
-- 3. Store the secret in a secure vault or Windows Credential Manager.
-- 4. Validate access and role mapping during deployment.

PRINT 'No privileged credential creation is included in this repository.';
GO
