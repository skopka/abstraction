# Skopka.Abstraction documentation guidance

## Products documentation synchronization

For consumer-visible API, options, defaults, contracts, package, security, or migration changes, update this repository's README, relevant docs and XML comments, and the Russian and English Products overviews, guides and working examples in the same work before release. If a change is purely internal, explicitly state why it has no documentation impact. Follow the canonical [Products documentation workflow](https://git.skopka.ru/skopka/products/src/branch/main/docs/documentation-workflow.md).

After the GitHub tag and all NuGet packages are published, verify them and update this family's released tag and full commit SHA in Products `scripts/fetch-sources.sh`; never pin moving `main` or present an unreleased feature as available. Review Products `README.md`, `abstraction/index.md`, `en/abstraction/index.md`, `guides/ru/abstraction.md` and its subpages, `guides/en/abstraction.md` and its subpages, `api-index/abstraction/index.md`, and navigation as needed. Run `sh scripts/build.sh` in Products to regenerate API only through DocFX and build the site; compile working examples against the supported release.

After deployment, verify the live Russian and English overview, guide and API pages at https://products.skopka.ru, including GitHub README and NuGet links and access without Basic Auth. Report the package version, tag and full SHA, Products commit, and verified URLs. If publication or deployment has not happened, report the remaining stage rather than claiming documentation is complete. Publication remains governed by the existing release authorization.
