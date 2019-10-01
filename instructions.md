# Org2 Software Development Technical Test

This technical test has been designed to allow you to solve a relatively simple problem and give candidates the freedom to choose the programming language and development tooling of their choice.

## Timing

Candidates have a ***24 hour*** window to participate in the test.

By ***09:00*** the day after the test is issued candidates must provide their artefacts to ***james.scanlan@nhs.net*** This can be in the form of an archive file containing their code or a link to a public code repository.

---

## The Technical Test

A company has invested in some automated deployment tooling which has recorded its deployment history in the following logical structure:

There are many projects.

```
Each project has a:

  project_id: A unique GUID

  project_group: A parent project group name of which the project is a member

  environments: A list of deployment environments that the project is deployed to,    for example:
		    Integration
		    Regression
		    QA
		    UAT
		    Live

  releases: one or more release. Each release details:

    version: a unique version identifier

    deployments: one or more deployments associated with a particular release.  Each deployment details:

      environment: the environment the release was deployed to

      created: a timestamp when the deployment took place

      state: whether the deployment was successful

      name: the name of the deployment
```

Versions are ordered from earliest to latest

Deployments are ordered from earliest to latest




## Example


```
{
  "projects": [
  {
      "project_id": "9f564a48-e40c-11e9-bc4f-acb57d6c5605",
      "project_group": "Spaniel",
      "environments": [
        {
          "environment": "Integration"
        },
        {
          "environment": "Test"
        },
        {
          "environment": "Live"
        }
      ],
      "releases": [
        {
          "version": "1.1.1.001",
          "deployments": [
            {
              "environment": "Integration",
              "created": "2019-10-01T06:40:01.000Z",
              "state": "Success",
              "name": "Deploy to Integration"
            },
            {
              "environment": "Test",
              "created": "2019-10-01T08:23:58.000Z",
              "state": "Success",
              "name": "Deploy to Test"
            },
            {
              "environment": "Live",
              "created": "2019-10-01T09:02:17.000Z",
              "state": "Success",
              "name": "Deploy to Live"
            }
          ]
        }
      ]
    }
  ]
}
```
---


## Questions

Please write working software which can answer the following questions.  Whilst some of these questions could be answered through a queries the expectation is that candidates write bespoke software instead.

1. How many successful deployments have taken place?
2. How does this break down by project group, by environment, by year?
3. Which is the most popular day of the week for live deployments? 
4. What is the average length of time a release takes from integration to live, by project group?
5. Please provide a break down by project group of success and unsuccessful deployments (successful being releases that aren't deployed to live), the number of deployments involved in the release pipeline and whether some environments had to be repeatedly deployed.


---

## Expected Artefacts from the Technical Test

This is an opportunity for candidates to show how they write software to solve problems.

Candidates are invited to write code in the language of their choice.

Submitted results are expected to:

>Answer the questions >> the results being written to a text file

>Be able to be executed by the assessors, with deployment packaging and/or documentation to detail how to execute the code.






